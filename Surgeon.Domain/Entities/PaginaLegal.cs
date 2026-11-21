using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

/// <summary>
/// Páginas legales editables desde el CMS (privacidad, términos, consentimiento informado).
/// La ruta es fija (la define la web); el CMS edita título, contenido y publicación.
/// </summary>
public class PaginaLegal : BaseEntity, ISeoEntity
{
    public string Clave { get; set; } = string.Empty;
    public string Ruta { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string ContenidoHtml { get; set; } = string.Empty;
    public bool Publicada { get; set; } = true;

    public string? MetaTitulo { get; set; }
    public string? MetaDescripcion { get; set; }
    public string? ImagenOgUrl { get; set; }
    public bool Indexable { get; set; } = true;
}
