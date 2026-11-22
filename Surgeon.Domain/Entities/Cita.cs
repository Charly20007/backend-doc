using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class Cita : BaseEntity
{
    public int IdPaciente { get; set; }
    public int IdServicioMedico { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
    public int IdEstadoCita { get; set; }
    public string? MotivoConsulta { get; set; }
    public string? NotasMedico { get; set; }
    public decimal? PrecioConsulta { get; set; }
    /// <summary>
    /// true mientras la cita ocupa su turno (no cancelada). La base de datos impide dos citas
    /// que ocupen turno con la misma hora de inicio (índice único parcial).
    /// </summary>
    public bool OcupaTurno { get; set; } = true;

    public Paciente? Paciente { get; set; }
    public ServicioMedico? ServicioMedico { get; set; }
    public EstadoCita? EstadoCita { get; set; }
}
