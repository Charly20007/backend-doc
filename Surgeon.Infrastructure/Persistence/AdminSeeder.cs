using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Settings;
using Surgeon.Domain.Common;
using Surgeon.Domain.Entities;

namespace Surgeon.Infrastructure.Persistence;

/// <summary>
/// Crea el primer usuario administrador si la tabla Usuarios está vacía,
/// usando la sección "AdminInicial" de la configuración. Así ninguna contraseña
/// queda escrita en los scripts SQL.
/// </summary>
public class AdminSeeder
{
    private readonly SurgeonDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly AdminInicialSettings _admin;
    private readonly ILogger<AdminSeeder> _logger;

    public AdminSeeder(SurgeonDbContext db, IPasswordHasher hasher, IOptions<AdminInicialSettings> admin, ILogger<AdminSeeder> logger)
    {
        _db = db;
        _hasher = hasher;
        _admin = admin.Value;
        _logger = logger;
    }

    public async Task EjecutarAsync()
    {
        if (await _db.Usuarios.AnyAsync())
            return;

        if (string.IsNullOrWhiteSpace(_admin.Correo) || string.IsNullOrWhiteSpace(_admin.Password))
        {
            _logger.LogWarning("No hay usuarios en la base de datos y falta la configuración AdminInicial: no se podrá iniciar sesión en el CMS.");
            return;
        }

        var rol = await _db.Roles.FirstOrDefaultAsync(r => r.Nombre == RolesSistema.Administrador);
        if (rol == null)
        {
            _logger.LogWarning("No existe el rol {Rol}; ejecute 02_Carga_Inicial.sql.", RolesSistema.Administrador);
            return;
        }

        _db.Usuarios.Add(new Usuario
        {
            NombreCompleto = _admin.NombreCompleto ?? "Administrador",
            Correo = _admin.Correo.Trim().ToLowerInvariant(),
            PasswordHash = _hasher.Hash(_admin.Password),
            IdRol = rol.Id,
            EsActivo = true,
        });
        await _db.SaveChangesAsync();
        _logger.LogInformation("Usuario administrador inicial creado: {Correo}", _admin.Correo);
    }
}
