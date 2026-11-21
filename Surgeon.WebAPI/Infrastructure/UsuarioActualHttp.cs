using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Surgeon.Application.Interfaces;

namespace Surgeon.WebAPI.Infrastructure;

/// <summary>Lee el usuario de la petición HTTP actual a partir de los claims del token JWT.</summary>
public class UsuarioActualHttp : IUsuarioActual
{
    private readonly IHttpContextAccessor _http;

    public UsuarioActualHttp(IHttpContextAccessor http)
    {
        _http = http;
    }

    private ClaimsPrincipal? Usuario => _http.HttpContext?.User;

    public int? Id => int.TryParse(Usuario?.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : null;
    public string? Correo => Usuario?.FindFirstValue(JwtRegisteredClaimNames.Email);
    public string? NombreCompleto => Usuario?.FindFirstValue(JwtRegisteredClaimNames.Name);
    public string? Rol => Usuario?.FindFirstValue("role");
    public string Ip => _http.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "::1";
}
