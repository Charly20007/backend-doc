using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class EstadoCita : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#3B82F6";
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}
