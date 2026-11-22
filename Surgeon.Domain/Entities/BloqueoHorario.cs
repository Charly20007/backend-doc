using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

/// <summary>Periodo en que no se atienden citas (vacaciones, congresos, días de quirófano).</summary>
public class BloqueoHorario : BaseEntity
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Motivo { get; set; } = string.Empty;
}
