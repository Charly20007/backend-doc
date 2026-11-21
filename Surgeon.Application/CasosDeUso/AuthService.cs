using Surgeon.Application.Common;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;

namespace Surgeon.Application.CasosDeUso;

/// <summary>Casos de uso de acceso al CMS: login, usuario actual y cambio de contraseña.</summary>
public class AuthService
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokens;
    private readonly IUsuarioActual _usuarioActual;

    public AuthService(IUnitOfWork uow, IPasswordHasher hasher, ITokenService tokens, IUsuarioActual usuarioActual)
    {
        _uow = uow;
        _hasher = hasher;
        _tokens = tokens;
        _usuarioActual = usuarioActual;
    }

    public async Task<Resultado<LoginRespuestaDto>> LoginAsync(LoginDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();
        var usuario = (await _uow.Usuarios.FindAsync(u => u.Correo == correo && u.EsActivo)).FirstOrDefault();

        // Mismo mensaje para usuario inexistente o contraseña errónea (no revelar qué correos existen)
        if (usuario == null || !_hasher.Verificar(dto.Password, usuario.PasswordHash))
            return Error.NoAutorizado("Correo o contraseña incorrectos.");

        var rol = await _uow.Roles.GetByIdAsync(usuario.IdRol);
        if (rol == null)
            return Error.NoAutorizado("El usuario no tiene un rol válido.");

        var (token, expira) = _tokens.GenerarToken(usuario, rol.Nombre);
        return new LoginRespuestaDto
        {
            Token = token,
            ExpiraEn = expira,
            Usuario = new UsuarioActualDto { Id = usuario.Id, NombreCompleto = usuario.NombreCompleto, Correo = usuario.Correo, Rol = rol.Nombre },
        };
    }

    public UsuarioActualDto UsuarioActual() => new()
    {
        Id = _usuarioActual.Id ?? 0,
        NombreCompleto = _usuarioActual.NombreCompleto ?? string.Empty,
        Correo = _usuarioActual.Correo ?? string.Empty,
        Rol = _usuarioActual.Rol ?? string.Empty,
    };

    public async Task<Resultado> CambiarPasswordAsync(CambiarPasswordDto dto)
    {
        if (_usuarioActual.Id is not int id)
            return Error.NoAutorizado("Sesión no válida.");

        var usuario = await _uow.Usuarios.GetByIdAsync(id);
        if (usuario == null || !_hasher.Verificar(dto.PasswordActual, usuario.PasswordHash))
            return Error.Validacion("La contraseña actual no es correcta.");

        usuario.PasswordHash = _hasher.Hash(dto.PasswordNueva);
        _uow.Usuarios.Update(usuario);
        await _uow.SaveChangesAsync();
        return Resultado.Ok();
    }
}
