using System.ComponentModel.DataAnnotations;

namespace Surgeon.Application.DTOs;

/// <summary>
/// Campos SEO comunes. Los límites duros (70/170) coinciden con la base de datos;
/// el CMS recomienda 60/160 para que Google no los corte.
/// </summary>
public class SeoDto
{
    [MaxLength(70, ErrorMessage = "El meta título no puede superar 70 caracteres.")]
    public string? MetaTitulo { get; set; }

    [MaxLength(170, ErrorMessage = "La meta descripción no puede superar 170 caracteres.")]
    public string? MetaDescripcion { get; set; }

    [MaxLength(500)]
    public string? ImagenOgUrl { get; set; }

    public bool Indexable { get; set; } = true;
}

public class PaginaSeoDto
{
    public int Id { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string Ruta { get; set; } = string.Empty;
    public string NombrePagina { get; set; } = string.Empty;
    public SeoDto Seo { get; set; } = new();
    public DateTime FechaModificacion { get; set; }
}

public class RedireccionDto
{
    public int Id { get; set; }
    public string RutaOrigen { get; set; } = string.Empty;
    public string RutaDestino { get; set; } = string.Empty;
    public bool EsPermanente { get; set; }
    public bool Activa { get; set; }
    public string? Nota { get; set; }
}

public class RedireccionGuardarDto
{
    [Required(ErrorMessage = "La ruta de origen es obligatoria.")]
    [MaxLength(300)]
    [RegularExpression(@"^/[^\s?#]*$", ErrorMessage = "La ruta de origen debe empezar con / y no llevar espacios, ? ni #.")]
    public string RutaOrigen { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ruta de destino es obligatoria.")]
    [MaxLength(300)]
    [RegularExpression(@"^(/|https?://)\S*$", ErrorMessage = "El destino debe empezar con / o con http(s)://.")]
    public string RutaDestino { get; set; } = string.Empty;

    public bool EsPermanente { get; set; } = true;
    public bool Activa { get; set; } = true;

    [MaxLength(200)]
    public string? Nota { get; set; }
}
