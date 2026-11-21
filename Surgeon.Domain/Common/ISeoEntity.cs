namespace Surgeon.Domain.Common;

/// <summary>
/// Campos SEO editables desde el CMS. Si MetaTitulo/MetaDescripcion están vacíos,
/// la web usa el nombre/resumen de la entidad como respaldo.
/// </summary>
public interface ISeoEntity
{
    string? MetaTitulo { get; set; }
    string? MetaDescripcion { get; set; }
    string? ImagenOgUrl { get; set; }
    /// <summary>false = la página lleva "noindex" y no aparece en el sitemap.</summary>
    bool Indexable { get; set; }
}
