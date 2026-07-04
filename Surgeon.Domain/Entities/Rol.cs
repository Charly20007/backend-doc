using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class Rol : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
