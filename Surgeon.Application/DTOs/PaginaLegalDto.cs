using System.ComponentModel.DataAnnotations;

namespace Surgeon.Application.DTOs;

public class PaginaLegalResumenDto
{
    public string Clave { get; set; } = string.Empty;
    public string Ruta { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public bool Publicada { get; set; }
    public DateTime FechaModificacion { get; set; }
}

public class PaginaLegalDto : PaginaLegalResumenDto
{
    public string ContenidoHtml { get; set; } = string.Empty;
    public SeoDto Seo { get; set; } = new();
}

public class PaginaLegalGuardarDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El contenido es obligatorio.")]
    public string ContenidoHtml { get; set; } = string.Empty;

    public bool Publicada { get; set; } = true;
    public SeoDto Seo { get; set; } = new();
}
