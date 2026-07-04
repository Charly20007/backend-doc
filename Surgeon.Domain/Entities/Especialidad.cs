using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class Especialidad : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Icono { get; set; }
    public ICollection<ServicioMedico> ServiciosMedicos { get; set; } = new List<ServicioMedico>();
}
