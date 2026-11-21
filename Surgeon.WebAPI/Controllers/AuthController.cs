using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers;

[Route("api/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly AuthService _auth;

    public AuthController(AuthService auth) => _auth = auth;

    /// <summary>Inicia sesión en el CMS y devuelve un token JWT.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<ActionResult<LoginRespuestaDto>> Login([FromBody] LoginDto dto) => Responder(await _auth.LoginAsync(dto));

    /// <summary>Datos del usuario de la sesión actual.</summary>
    [HttpGet("me")]
    [Authorize]
    public ActionResult<UsuarioActualDto> Me() => Ok(_auth.UsuarioActual());

    /// <summary>Cambia la contraseña del usuario de la sesión actual.</summary>
    [HttpPut("password")]
    [Authorize]
    [EnableRateLimiting("login")]
    public async Task<ActionResult> CambiarPassword([FromBody] CambiarPasswordDto dto) => SinContenido(await _auth.CambiarPasswordAsync(dto));
}
