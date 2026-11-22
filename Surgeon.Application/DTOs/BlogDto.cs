using System.ComponentModel.DataAnnotations;

namespace Surgeon.Application.DTOs;

public class CategoriaBlogDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}

public class CategoriaBlogGuardarDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(100)]
    [RegularExpression(@"^[a-z0-9]+(-[a-z0-9]+)*$", ErrorMessage = "El slug solo puede tener minúsculas, números y guiones.")]
    public string? Slug { get; set; }
}

public class ServicioRelacionadoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}

/// <summary>Mismo formato que usa la web (ArticuloMedico) + SEO.</summary>
public class ArticuloBlogDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Resumen { get; set; } = string.Empty;
    public string ContenidoHtml { get; set; } = string.Empty;
    public string? ImagenPortadaUrl { get; set; }
    public CategoriaBlogDto Categoria { get; set; } = new();
    public ServicioRelacionadoDto? ServicioRelacionado { get; set; }
    public DateTime FechaPublicacion { get; set; }
    public DateTime FechaModificacion { get; set; }
    public string Autor { get; set; } = string.Empty;
    public int TiempoLectura { get; set; }
    public int Vistas { get; set; }
    public bool PublicadoEnWeb { get; set; }
    public bool Destacado { get; set; }
    public SeoDto Seo { get; set; } = new();
}

public class DestacarDto
{
    public bool Destacado { get; set; }
}

public class ArticuloBlogGuardarDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(200)]
    [RegularExpression(@"^[a-z0-9]+(-[a-z0-9]+)*$", ErrorMessage = "El slug solo puede tener minúsculas, números y guiones.")]
    public string? Slug { get; set; }

    [Required(ErrorMessage = "El resumen es obligatorio.")]
    [MaxLength(500)]
    public string Resumen { get; set; } = string.Empty;

    [Required(ErrorMessage = "El contenido es obligatorio.")]
    public string ContenidoHtml { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ImagenPortadaUrl { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría.")]
    public int IdCategoria { get; set; }

    public int? IdServicioMedico { get; set; }

    [MaxLength(100)]
    public string? Autor { get; set; }

    /// <summary>Si es null se calcula a partir del contenido (~200 palabras por minuto).</summary>
    [Range(1, 120)]
    public int? TiempoLecturaMinutos { get; set; }

    public DateTime? FechaPublicacion { get; set; }
    public bool PublicadoEnWeb { get; set; } = true;
    public SeoDto Seo { get; set; } = new();
}
