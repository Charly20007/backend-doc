# Arquitectura – Clean Architecture

La API está dividida en **4 proyectos (capas)**. La regla principal: **las dependencias apuntan hacia adentro**. Las capas internas (el negocio) no saben nada de las externas (HTTP, base de datos).

```
        ┌──────────────────────────────────────────────┐
        │  Surgeon.WebAPI          (HTTP)              │  controladores, JWT, CORS, Swagger
        │   ┌──────────────────────────────────────┐   │
        │   │  Surgeon.Infrastructure  (técnico)   │   │  PostgreSQL, EF Core, BCrypt, JWT
        │   │   ┌──────────────────────────────┐   │   │
        │   │   │  Surgeon.Application (casos  │   │   │  qué hace el sistema
        │   │   │  de uso)                     │   │   │
        │   │   │   ┌──────────────────────┐   │   │   │
        │   │   │   │  Surgeon.Domain      │   │   │   │  qué es el negocio
        │   │   │   │  (entidades)         │   │   │   │
        │   │   │   └──────────────────────┘   │   │   │
        │   │   └──────────────────────────────┘   │   │
        │   └──────────────────────────────────────┘   │
        └──────────────────────────────────────────────┘
```

| Capa | Depende de | Responsabilidad | No debe |
|---|---|---|---|
| **Domain** | nada | Entidades (`ServicioMedico`, `Cita`…) y conceptos del negocio (`RolesSistema`, `TipoContenidoServicio`) | Conocer la base de datos, HTTP ni librerías externas |
| **Application** | Domain | **Casos de uso**: las reglas de negocio ("no puede haber dos procedimientos con la misma URL", "al cambiar el slug se crea una redirección 301"). Define **interfaces** de lo que necesita (`IUnitOfWork`, `IPasswordHasher`, `IUsuarioActual`…) | Usar EF Core, ASP.NET ni SQL |
| **Infrastructure** | Application, Domain | **Implementa** esas interfaces: PostgreSQL con EF Core, repositorios, BCrypt, JWT, aviso a la web, auditoría automática | Contener reglas de negocio |
| **WebAPI** | Application, Infrastructure | Recibe HTTP, valida el formato de la entrada, llama al caso de uso y traduce el resultado a una respuesta HTTP | **Contener lógica**: los controladores solo traducen |

## Recorrido de una petición

Ejemplo: el CMS guarda un procedimiento con `PUT /api/admin/servicios/5`.

```
CMS ──HTTP──► AdminServiciosController.Actualizar          (WebAPI)
                 │  1. [ApiController] valida el DTO (campos obligatorios, largos)
                 ▼
              ProcedimientosService.ActualizarAsync         (Application)
                 │  2. ¿existe? ¿especialidad válida? ¿slug repetido?  → Error.NoEncontrado / Conflicto
                 │  3. aplica cambios, registra redirección si cambió el slug
                 │  4. _uow.SaveChangesAsync()  ── usa la interfaz, no sabe que es PostgreSQL
                 ▼
              UnitOfWork / SurgeonDbContext                 (Infrastructure)
                 │  5. AuditoriaInterceptor completa usuario, IP y fecha
                 │  6. EF Core genera el SQL y lo ejecuta en PostgreSQL
                 ▼
              Resultado<ServicioDetalleDto>  ──►  ApiControllerBase.Responder()
                                                   7. Éxito → 200 · NoEncontrado → 404 · Conflicto → 409 · Validación → 400
```

Un controlador completo queda así, sin lógica:

```csharp
[HttpPut("{id:int}")]
public async Task<ActionResult<ServicioDetalleDto>> Actualizar(int id, [FromBody] ServicioGuardarDto dto) =>
    Responder(await _procedimientos.ActualizarAsync(id, dto));
```

## Piezas clave

| Pieza | Dónde | Para qué |
|---|---|---|
| `Resultado` / `Resultado<T>` y `Error` | `Application/Common/Resultado.cs` | Los casos de uso devuelven éxito o un error de negocio (sin lanzar excepciones para casos esperados) |
| `ApiControllerBase` | `WebAPI/Infrastructure` | Traduce `Resultado` a HTTP con el cuerpo `{ mensaje }` |
| `IUnitOfWork` + `IRepository<T>` | `Application/Interfaces` → `Infrastructure/Repositories` | Acceso a datos sin depender de EF Core en Application |
| `IUsuarioActual` | `Application/Interfaces` → `WebAPI/Infrastructure/UsuarioActualHttp` | Quién hace la petición, sin que Application conozca HTTP |
| `AuditoriaInterceptor` | `Infrastructure/Persistence` | Rellena `usuario_modificacion`, `ip_modificacion` y `fecha_modificacion` en cada guardado |
| `AddApplication()` / `AddInfrastructure()` | `DependencyInjection.cs` de cada capa | Cada capa registra sus propias dependencias; `Program.cs` solo las llama |

## Casos de uso (Application/CasosDeUso)

| Servicio | Lo usan |
|---|---|
| `ProcedimientosService` | `ServiciosController`, `AdminServiciosController` |
| `BlogService` | `BlogController`, `AdminBlogController` |
| `TestimoniosService` | `TestimoniosController`, `AdminTestimoniosController` |
| `ConfiguracionService` | `ConfiguracionController`, `AdminConfiguracionController` |
| `SeoService` | `SeoController`, `AdminSeoController` |
| `PaginasLegalesService` | `PaginasLegalesController`, `AdminPaginasLegalesController` |
| `CitasService` | `CitasController` |
| `AuthService` | `AuthController` |
| `CatalogosService` | `AdminCatalogosController` |
| `RedireccionesService` | Usado por Procedimientos y Blog al cambiar o borrar una URL |

## Cómo agregar una funcionalidad nueva

Ejemplo: "Bloqueos de horario" (vacaciones del doctor).

1. **Domain:** la entidad ya existe (`BloqueoHorario`). Si faltara, se crea aquí.
2. **Infrastructure:** si cambia la estructura de la base, agregar el `DbSet` y crear la migración (`dotnet ef migrations add …`). Exponer el repositorio en `IUnitOfWork`.
3. **Application:**
   - DTOs en `DTOs/` (con validaciones `[Required]`, `[MaxLength]`…).
   - Caso de uso en `CasosDeUso/BloqueosService.cs`, que devuelve `Resultado<T>`.
   - Registrarlo en `Application/DependencyInjection.cs`.
4. **WebAPI:** un controlador delgado que llame al servicio con `Responder(...)`.

## Lo que no se considera lógica (y sí va en WebAPI)

- Rutas, verbos HTTP y códigos de respuesta.
- `[Authorize]` (quién puede llamar) y límites de peticiones (`[EnableRateLimiting]`).
- Validación del **formato** de la entrada con atributos en los DTOs (lo hace `[ApiController]` automáticamente).
- Avisar a la web tras un cambio (`RevalidarWebFilter`): es un efecto técnico transversal, no una regla de negocio.
