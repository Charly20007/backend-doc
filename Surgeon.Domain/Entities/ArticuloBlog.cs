using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class ArticuloBlog : BaseEntity
{
    public string Titulo { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Resumen { get; set; } = string.Empty;
    public string ContenidoHtml { get; set; } = string.Empty;
    public string? ImagenPortadaUrl { get; set; }
    public int IdCategoria { get; set; }
    public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;
    public bool PublicadoEnWeb { get; set; } = true;
    public int Vistas { get; set; } = 0;

    public CategoriaBlog? Categoria { get; set; }
}
