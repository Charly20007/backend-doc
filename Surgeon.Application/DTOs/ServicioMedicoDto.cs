using System.ComponentModel.DataAnnotations;

namespace Surgeon.Application.DTOs;

/// <summary>Versión ligera para listados y tarjetas.</summary>
public class ServicioMedicoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? NombreCorto { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Resumen { get; set; } = string.Empty;
    public string? DescripcionDetallada { get; set; }
    public int TiempoEstimadoMinutos { get; set; }
    public decimal? PrecioReferencial { get; set; }
    public string? Icono { get; set; }
    public int IdEspecialidad { get; set; }
    public string EspecialidadNombre { get; set; } = string.Empty;
    public bool DestacadoEnWeb { get; set; }
    public int Orden { get; set; }
    public bool Indexable { get; set; }
    public DateTime FechaModificacion { get; set; }
}

/// <summary>Detalle completo para la página /procedimientos/[slug] y el editor del CMS.</summary>
public class ServicioDetalleDto : ServicioMedicoDto
{
    public string? CondicionTratada { get; set; }
    public string? TituloIndicaciones { get; set; }
    public SeoDto Seo { get; set; } = new();
    public List<ContenidoItemDto> Introduccion { get; set; } = new();
    public List<ContenidoItemDto> Indicaciones { get; set; } = new();
    public List<ContenidoItemDto> Pasos { get; set; } = new();
    public List<ContenidoItemDto> Recuperacion { get; set; } = new();
    public List<ContenidoItemDto> Preguntas { get; set; } = new();
}

public class ContenidoItemDto
{
    [MaxLength(200)]
    public string? Titulo { get; set; }

    [Required(ErrorMessage = "El texto del bloque es obligatorio.")]
    [MaxLength(2000)]
    public string Texto { get; set; } = string.Empty;
}

/// <summary>Crear o editar un procedimiento desde el CMS.</summary>
public class ServicioGuardarDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(80)]
    public string? NombreCorto { get; set; }

    /// <summary>Si se deja vacío se genera a partir del nombre.</summary>
    [MaxLength(150)]
    [RegularExpression(@"^[a-z0-9]+(-[a-z0-9]+)*$", ErrorMessage = "El slug solo puede tener minúsculas, números y guiones (ej. cirugia-de-vesicula).")]
    public string? Slug { get; set; }

    [Required(ErrorMessage = "El resumen es obligatorio.")]
    [MaxLength(300)]
    public string Resumen { get; set; } = string.Empty;

    public string? DescripcionDetallada { get; set; }

    [Range(5, 1440, ErrorMessage = "La duración debe estar entre 5 y 1440 minutos.")]
    public int TiempoEstimadoMinutos { get; set; } = 45;

    [Range(0, 9999999, ErrorMessage = "El precio no es válido.")]
    public decimal? PrecioReferencial { get; set; }

    [MaxLength(50)]
    public string? Icono { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una especialidad.")]
    public int IdEspecialidad { get; set; }

    public bool DestacadoEnWeb { get; set; } = true;
    public int Orden { get; set; }

    [MaxLength(150)]
    public string? CondicionTratada { get; set; }

    [MaxLength(150)]
    public string? TituloIndicaciones { get; set; }

    public SeoDto Seo { get; set; } = new();

    public List<ContenidoItemDto> Introduccion { get; set; } = new();
    public List<ContenidoItemDto> Indicaciones { get; set; } = new();
    public List<ContenidoItemDto> Pasos { get; set; } = new();
    public List<ContenidoItemDto> Recuperacion { get; set; } = new();
    public List<ContenidoItemDto> Preguntas { get; set; } = new();
}
