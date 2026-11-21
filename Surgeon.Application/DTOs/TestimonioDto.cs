using System.ComponentModel.DataAnnotations;

namespace Surgeon.Application.DTOs;

public class TestimonioDto
{
    public int Id { get; set; }
    public string NombrePaciente { get; set; } = string.Empty;
    public string Procedimiento { get; set; } = string.Empty;
    public int? IdServicioMedico { get; set; }
    public string? ProcedimientoSlug { get; set; }
    public int CalificacionEstrellas { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public DateTime FechaTestimonio { get; set; }
    public bool AprobadoParaWeb { get; set; }
}

public class TestimonioGuardarDto
{
    [Required(ErrorMessage = "El nombre del paciente es obligatorio.")]
    [MaxLength(100)]
    public string NombrePaciente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El procedimiento es obligatorio.")]
    [MaxLength(150)]
    public string Procedimiento { get; set; } = string.Empty;

    public int? IdServicioMedico { get; set; }

    [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
    public int CalificacionEstrellas { get; set; } = 5;

    [Required(ErrorMessage = "El comentario es obligatorio.")]
    [MaxLength(1000)]
    public string Comentario { get; set; } = string.Empty;

    public DateTime? FechaTestimonio { get; set; }
    public bool AprobadoParaWeb { get; set; }
}
