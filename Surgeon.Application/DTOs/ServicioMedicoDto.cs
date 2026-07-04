namespace Surgeon.Application.DTOs;

public class ServicioMedicoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Resumen { get; set; } = string.Empty;
    public string? DescripcionDetallada { get; set; }
    public int TiempoEstimadoMinutos { get; set; }
    public decimal? PrecioReferencial { get; set; }
    public string? Icono { get; set; }
    public string EspecialidadNombre { get; set; } = string.Empty;
    public bool DestacadoEnWeb { get; set; }
}
