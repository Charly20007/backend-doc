namespace Surgeon.Application.DTOs;

public class TestimonioDto
{
    public int Id { get; set; }
    public string NombrePaciente { get; set; } = string.Empty;
    public string Procedimiento { get; set; } = string.Empty;
    public int CalificacionEstrellas { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public DateTime FechaTestimonio { get; set; }
}
