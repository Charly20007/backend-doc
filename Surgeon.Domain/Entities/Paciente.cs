using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class Paciente : BaseEntity
{
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? NumeroDocumento { get; set; }
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public string? AntecedentesRelevantes { get; set; }

    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}
