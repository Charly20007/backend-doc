# Surgeon Web API

API .NET de la web del Dr. Juan Carlos Marcos Enriquez: contenido público, SEO administrable y CMS.

## Puesta en marcha

1. **PostgreSQL** (en Docker):
   ```bash
   docker run -d --name surgeon-pg -e POSTGRES_USER=surgeon -e POSTGRES_PASSWORD=<contraseña> -e POSTGRES_DB=surgeon -p 5432:5432 -v surgeon-pg-data:/var/lib/postgresql/data postgres:16-alpine
   ```
2. **Configuración local:** copie `Surgeon.WebAPI/appsettings.Development.local.example.json` como
   `appsettings.Development.local.json` (no se sube a git) y complete la conexión, la clave JWT,
   el administrador inicial y el secreto de revalidación.
3. **Ejecutar:** `docker start surgeon-pg` y luego, en `Surgeon.WebAPI`, `dotnet run` → http://localhost:5029/swagger
   - Al arrancar, la API **crea o actualiza las tablas sola** (migraciones de Entity Framework).
4. **Contenido inicial** (solo en una base nueva): `docker exec -i surgeon-pg psql -U surgeon -d surgeon < Surgeon.Infrastructure/Persistence/Datos/datos_iniciales.sql`

La estructura de la base está documentada en [BASE_DE_DATOS.md](BASE_DE_DATOS.md) y la organización del código (Clean Architecture) en [ARQUITECTURA.md](ARQUITECTURA.md).

### Cambios en la estructura de la base

Se hacen con migraciones de EF Core (no con scripts a mano):

```bash
dotnet tool restore
dotnet ef migrations add NombreDelCambio --project Surgeon.Infrastructure --startup-project Surgeon.WebAPI --output-dir Persistence/Migraciones
```

La API aplica la migración nueva la próxima vez que arranca.

## Acceso al CMS

Al iniciar, si la tabla `Usuarios` está vacía, la API crea el administrador definido en la sección
`AdminInicial` de la configuración. En desarrollo está en `appsettings.Development.local.json` (archivo local, ignorado por git).

**Producción:** no use esos valores. Defina variables de entorno:

| Variable | Uso |
|---|---|
| `Jwt__Key` | Clave secreta de firma (≥ 32 caracteres, aleatoria) |
| `AdminInicial__Correo` / `AdminInicial__Password` | Primer administrador (solo se usa si no hay usuarios) |
| `ConnectionStrings__DefaultConnection` | Conexión a SQL Server |
| `Cors__Origenes__0`, `Cors__Origenes__1`… | Dominios de la web y del CMS |

## Endpoints

- **Públicos** (`/api/servicios`, `/api/blog`, `/api/testimonios`, `/api/configuracion`, `/api/seo`, `POST /api/citas`): los usa la web.
- **CMS** (`/api/admin/...`, `GET/PUT /api/citas`): requieren `Authorization: Bearer <token>` de `POST /api/auth/login`.
  - Contenido y SEO: roles *Administrador* y *Cirujano*.
  - Agenda de citas: además *Secretaria*.
- Login limitado a 5 intentos por minuto por IP; solicitudes de cita a 10 cada 10 minutos.
- Al cambiar el slug de un procedimiento o artículo se crea automáticamente una redirección 301 desde la URL anterior.

Ejemplos listos para usar en `Surgeon.WebAPI/Surgeon.WebAPI.http`.
