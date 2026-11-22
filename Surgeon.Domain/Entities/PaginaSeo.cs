using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

/// <summary>
/// SEO de las páginas fijas de la web (portada, contacto, perfil del doctor, listados...).
/// Las páginas de procedimientos y artículos guardan su SEO en su propia entidad.
/// </summary>
public class PaginaSeo : BaseEntity, ISeoEntity
{
    /// <summary>Identificador fijo usado por la web, ej. "inicio", "contacto".</summary>
    public string Clave { get; set; } = string.Empty;
    /// <summary>Ruta pública, ej. "/contacto". Informativa para el CMS.</summary>
    public string Ruta { get; set; } = string.Empty;
    public string NombrePagina { get; set; } = string.Empty;

    // Textos visibles (si están vacíos, la web usa los suyos)
    /// <summary>Título principal (h1) de la página.</summary>
    public string? Titulo { get; set; }
    /// <summary>Frase bajo el título.</summary>
    public string? Descripcion { get; set; }
    /// <summary>Título de una sección secundaria, ej. la columna lateral del blog.</summary>
    public string? TituloSecundario { get; set; }

    public string? MetaTitulo { get; set; }
    public string? MetaDescripcion { get; set; }
    public string? ImagenOgUrl { get; set; }
    public bool Indexable { get; set; } = true;
}
