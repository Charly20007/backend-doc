using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class BloqueoHorario : BaseEntity
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Motivo { get; set; } = string.Empty;
}
