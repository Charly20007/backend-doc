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

/// <summary>Testimonio con los datos internos que solo ve el CMS (el contacto nunca sale en la web).</summary>
public class TestimonioAdminDto : TestimonioDto
{
    public bool EnviadoDesdeWeb { get; set; }
    public bool PendienteRevision { get; set; }
    public bool AutorizaPublicacion { get; set; }
    public string? ContactoPaciente { get; set; }
}

/// <summary>Testimonio que envía un paciente desde la web. Queda pendiente hasta que el administrador lo apruebe.</summary>
public class TestimonioEnviarDto
{
    [Required(ErrorMessage = "Escribe tu nombre.")]
    [MaxLength(100)]
    public string NombrePaciente { get; set; } = string.Empty;

    public int? IdServicioMedico { get; set; }

    [MaxLength(150)]
    public string? Procedimiento { get; set; }

    [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
    public int CalificacionEstrellas { get; set; } = 5;

    [Required(ErrorMessage = "Escribe tu comentario.")]
    [MinLength(20, ErrorMessage = "Cuéntanos un poco más (mínimo 20 caracteres).")]
    [MaxLength(1000)]
    public string Comentario { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? ContactoPaciente { get; set; }

    public bool AutorizaPublicacion { get; set; }

    /// <summary>Campo trampa: invisible para las personas, los bots lo llenan.</summary>
    [MaxLength(200)]
    public string? Sitio { get; set; }
}

public class PendientesDto
{
    public int Cantidad { get; set; }
}
