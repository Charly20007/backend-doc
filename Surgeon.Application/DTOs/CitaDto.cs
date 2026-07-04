namespace Surgeon.Application.DTOs;

public class CrearCitaDto
{
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public int IdServicioMedico { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public string? MotivoConsulta { get; set; }
}

public class CitaRespuestaDto
{
    public int Id { get; set; }
    public string PacienteNombreCompleto { get; set; } = string.Empty;
    public string ServicioMedicoNombre { get; set; } = string.Empty;
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
    public string EstadoCitaNombre { get; set; } = string.Empty;
    public string EstadoColorHex { get; set; } = string.Empty;
    public string? MotivoConsulta { get; set; }
}
