using System.ComponentModel.DataAnnotations;

namespace Surgeon.Application.DTOs;

public class CrearCitaDto
{
    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [MaxLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [MaxLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    [MaxLength(100)]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^\+?[0-9 ()-]{6,20}$", ErrorMessage = "El teléfono no es válido.")]
    public string Telefono { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un procedimiento.")]
    public int IdServicioMedico { get; set; }

    public DateTime FechaHoraInicio { get; set; }

    [MaxLength(500)]
    public string? MotivoConsulta { get; set; }
}

public class CitaRespuestaDto
{
    public int Id { get; set; }
    public string PacienteNombreCompleto { get; set; } = string.Empty;
    public string? PacienteTelefono { get; set; }
    public string? PacienteCorreo { get; set; }
    public string ServicioMedicoNombre { get; set; } = string.Empty;
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
    public string EstadoCitaNombre { get; set; } = string.Empty;
    public string EstadoColorHex { get; set; } = string.Empty;
    public string? MotivoConsulta { get; set; }
}

public class CambiarEstadoCitaDto
{
    [Required(ErrorMessage = "El estado es obligatorio.")]
    public string NuevoEstadoNombre { get; set; } = string.Empty;
}
