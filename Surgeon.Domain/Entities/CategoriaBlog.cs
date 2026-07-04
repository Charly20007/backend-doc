using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class CategoriaBlog : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public ICollection<ArticuloBlog> Articulos { get; set; } = new List<ArticuloBlog>();
}
