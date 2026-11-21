using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.JsonWebTokens;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;

namespace Surgeon.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokens;

    public AuthController(IUnitOfWork uow, IPasswordHasher hasher, ITokenService tokens)
    {
        _uow = uow;
        _hasher = hasher;
        _tokens = tokens;
    }

    /// <summary>Inicia sesión en el CMS y devuelve un token JWT.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<ActionResult<LoginRespuestaDto>> Login([FromBody] LoginDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();
        var usuario = (await _uow.Usuarios.FindAsync(u => u.Correo == correo && u.EsActivo)).FirstOrDefault();

        // Mismo mensaje para usuario inexistente o contraseña errónea (no revelar qué correos existen)
        if (usuario == null || !_hasher.Verificar(dto.Password, usuario.PasswordHash))
            return Unauthorized(new { mensaje = "Correo o contraseña incorrectos." });

        var rol = await _uow.Roles.GetByIdAsync(usuario.IdRol);
        if (rol == null)
            return Unauthorized(new { mensaje = "El usuario no tiene un rol válido." });

        var (token, expira) = _tokens.GenerarToken(usuario, rol.Nombre);
        return Ok(new LoginRespuestaDto
        {
            Token = token,
            ExpiraEn = expira,
            Usuario = new UsuarioActualDto { Id = usuario.Id, NombreCompleto = usuario.NombreCompleto, Correo = usuario.Correo, Rol = rol.Nombre },
        });
    }

    /// <summary>Datos del usuario de la sesión actual.</summary>
    [HttpGet("me")]
    [Authorize]
    public ActionResult<UsuarioActualDto> Me()
    {
        return Ok(new UsuarioActualDto
        {
            Id = int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : 0,
            NombreCompleto = User.FindFirstValue(JwtRegisteredClaimNames.Name) ?? string.Empty,
            Correo = User.FindFirstValue(JwtRegisteredClaimNames.Email) ?? string.Empty,
            Rol = User.FindFirstValue("role") ?? string.Empty,
        });
    }

    /// <summary>Cambia la contraseña del usuario de la sesión actual.</summary>
    [HttpPut("password")]
    [Authorize]
    [EnableRateLimiting("login")]
    public async Task<ActionResult> CambiarPassword([FromBody] CambiarPasswordDto dto)
    {
        if (!int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id))
            return Unauthorized();

        var usuario = await _uow.Usuarios.GetByIdAsync(id);
        if (usuario == null || !_hasher.Verificar(dto.PasswordActual, usuario.PasswordHash))
            return BadRequest(new { mensaje = "La contraseña actual no es correcta." });

        usuario.PasswordHash = _hasher.Hash(dto.PasswordNueva);
        _uow.Usuarios.Update(usuario);
        await _uow.SaveChangesAsync();
        return NoContent();
    }
}
