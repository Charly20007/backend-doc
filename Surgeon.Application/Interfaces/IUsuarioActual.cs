namespace Surgeon.Application.Interfaces;

/// <summary>
/// Quién hace la petición actual. La implementa la capa WebAPI (a partir del token JWT)
/// para que Application e Infrastructure no dependan de ASP.NET.
/// </summary>
public interface IUsuarioActual
{
    int? Id { get; }
    string? Correo { get; }
    string? NombreCompleto { get; }
    string? Rol { get; }
    string Ip { get; }
}
