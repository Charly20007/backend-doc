using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Settings;
using Surgeon.Domain.Entities;

namespace Surgeon.Infrastructure.Security;

public class JwtTokenService : ITokenService
{
    private readonly JwtSettings _settings;

    public JwtTokenService(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public (string Token, DateTime ExpiraEn) GenerarToken(Usuario usuario, string rol)
    {
        var expira = DateTime.UtcNow.AddHours(_settings.ExpiracionHoras);
        var credenciales = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
            SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            Expires = expira,
            SigningCredentials = credenciales,
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
                new Claim(JwtRegisteredClaimNames.Name, usuario.NombreCompleto),
                new Claim("role", rol),
            }),
        };

        return (new JsonWebTokenHandler().CreateToken(descriptor), expira);
    }
}
