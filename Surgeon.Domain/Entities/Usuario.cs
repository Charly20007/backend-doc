using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class Usuario : BaseEntity
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public int IdRol { get; set; }
    public bool EsActivo { get; set; } = true;

    public Rol? Rol { get; set; }
}
