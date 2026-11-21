using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Surgeon.Domain.Common;

namespace Surgeon.WebAPI.Infrastructure;

/// <summary>
/// Base de los controladores del CMS: exige sesión iniciada y completa la auditoría
/// (UsuarioModificacion / IPModificacion) con el usuario y la IP de la petición.
/// </summary>
[ApiController]
[Authorize(Roles = RolesSistema.Contenido)]
public abstract class AdminControllerBase : ControllerBase
{
    protected int? UsuarioId =>
        int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

    protected void Auditar(BaseEntity entidad)
    {
        var correo = User.FindFirstValue(JwtRegisteredClaimNames.Email) ?? User.FindFirstValue(ClaimTypes.Email) ?? "cms";
        var usuario = correo.Split('@')[0];
        entidad.UsuarioModificacion = usuario.Length > 20 ? usuario[..20] : usuario;

        var ip = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "::1";
        entidad.IPModificacion = ip.Length > 15 ? ip[..15] : ip;
        entidad.FechaModificacion = DateTime.UtcNow;
    }

    protected ActionResult Conflicto(string mensaje) => Conflict(new { mensaje });
    protected ActionResult NoEncontrado(string mensaje) => NotFound(new { mensaje });
}
