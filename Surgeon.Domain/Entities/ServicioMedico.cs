using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class ServicioMedico : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Resumen { get; set; } = string.Empty;
    public string? DescripcionDetallada { get; set; }
    public int TiempoEstimadoMinutos { get; set; } = 45;
    public decimal? PrecioReferencial { get; set; }
    public string? Icono { get; set; }
    public int IdEspecialidad { get; set; }
    public bool DestacadoEnWeb { get; set; } = true;

    public Especialidad? Especialidad { get; set; }
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}
