using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class HorarioAtencion : BaseEntity
{
    public int DiaSemana { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public bool EsActivo { get; set; } = true;
}
