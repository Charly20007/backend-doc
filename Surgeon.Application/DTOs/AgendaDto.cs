using System.ComponentModel.DataAnnotations;

namespace Surgeon.Application.DTOs;

/// <summary>Turnos libres para el formulario de citas de la web.</summary>
public class DisponibilidadDto
{
    public bool ReservasActivas { get; set; }
    public int DuracionTurnoMinutos { get; set; }
    /// <summary>Solo los días que tienen al menos un turno libre.</summary>
    public List<DiaDisponibleDto> Dias { get; set; } = new();
}

public class DiaDisponibleDto
{
    /// <summary>yyyy-MM-dd</summary>
    public string Fecha { get; set; } = string.Empty;
    /// <summary>Horas de inicio libres, "HH:mm".</summary>
    public List<string> Turnos { get; set; } = new();
}

public class BloqueoDto
{
    public int Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Motivo { get; set; } = string.Empty;
}

public class BloqueoGuardarDto
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    [Required(ErrorMessage = "Indique el motivo (solo lo ve el consultorio).")]
    [MaxLength(255)]
    public string Motivo { get; set; } = string.Empty;
}
