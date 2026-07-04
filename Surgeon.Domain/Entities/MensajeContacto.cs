using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class MensajeContacto : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Asunto { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public bool Leido { get; set; } = false;
    public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;
}
