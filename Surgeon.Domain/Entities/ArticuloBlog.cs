using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class ArticuloBlog : BaseEntity, ISeoEntity
{
    public string Titulo { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Resumen { get; set; } = string.Empty;
    public string ContenidoHtml { get; set; } = string.Empty;
    public string? ImagenPortadaUrl { get; set; }
    public int IdCategoria { get; set; }
    /// <summary>Procedimiento relacionado: el artículo enlaza a su página y viceversa.</summary>
    public int? IdServicioMedico { get; set; }
    public string Autor { get; set; } = "Dr. Juan Carlos Marcos Enriquez";
    public int TiempoLecturaMinutos { get; set; } = 5;
    public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;
    public bool PublicadoEnWeb { get; set; } = true;
    public int Vistas { get; set; } = 0;
    /// <summary>Se muestra en grande en el blog. Solo un artículo puede estar destacado.</summary>
    public bool Destacado { get; set; } = false;

    // SEO
    public string? MetaTitulo { get; set; }
    public string? MetaDescripcion { get; set; }
    public string? ImagenOgUrl { get; set; }
    public bool Indexable { get; set; } = true;

    public CategoriaBlog? Categoria { get; set; }
    public ServicioMedico? ServicioMedico { get; set; }
}
