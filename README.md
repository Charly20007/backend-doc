# Surgeon Web API

API .NET de la web del Dr. Juan Carlos Marcos Enriquez: contenido público, SEO administrable y CMS.

## Puesta en marcha

1. **SQL Server.** En macOS no existe LocalDB; se puede usar Docker:
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<una-contraseña-segura>" -p 1433:1433 --name surgeon-sql -d mcr.microsoft.com/mssql/server:2022-latest
   ```
   y crear `Surgeon.WebAPI/appsettings.Development.local.json` (no se sube a git) copiando
   `appsettings.Development.local.example.json` y completando la conexión, la clave JWT, el admin inicial y el secreto de revalidación:
   ```json
   "ConnectionStrings": { "DefaultConnection": "Server=localhost,1433;Database=SurgeonDb;User Id=sa;Password=<...>;TrustServerCertificate=True" }
   ```
2. **Scripts** en `Surgeon.Infrastructure/Persistence/TablesAndData/`:
   - Base nueva: `00_Cleanup.sql` → `01_Tabla_Maestra.sql` → `02_Carga_Inicial.sql` → `03_Fase1_SEO.sql`
   - Base existente: solo `03_Fase1_SEO.sql` (es idempotente).
3. **Ejecutar:** `docker start surgeon-sql` y luego `dotnet run --project Surgeon.WebAPI` → http://localhost:5029/swagger

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
