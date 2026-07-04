using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class Testimonio : BaseEntity
{
    public string NombrePaciente { get; set; } = string.Empty;
    public string Procedimiento { get; set; } = string.Empty;
    public int CalificacionEstrellas { get; set; } = 5;
    public string Comentario { get; set; } = string.Empty;
    public DateTime FechaTestimonio { get; set; } = DateTime.UtcNow;
    public bool AprobadoParaWeb { get; set; } = true;
}
