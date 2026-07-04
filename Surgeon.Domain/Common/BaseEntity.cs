namespace Surgeon.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public string UsuarioModificacion { get; set; } = "UsrSurgeon";
    public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    public string IPModificacion { get; set; } = "::1";
    public bool Eliminado { get; set; } = false;
}
