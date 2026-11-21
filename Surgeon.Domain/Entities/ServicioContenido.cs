using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

/// <summary>Tipos de bloque de contenido de la página de un procedimiento.</summary>
public static class TipoContenidoServicio
{
    public const string Introduccion = "Introduccion"; // Texto
    public const string Indicacion = "Indicacion";     // Texto (síntoma o indicación)
    public const string Paso = "Paso";                 // Titulo + Texto
    public const string Recuperacion = "Recuperacion"; // Texto
    public const string Pregunta = "Pregunta";         // Titulo (pregunta) + Texto (respuesta)

    public static readonly string[] Todos = { Introduccion, Indicacion, Paso, Recuperacion, Pregunta };
}

/// <summary>
/// Bloque de contenido de la página de un procedimiento (párrafo, síntoma, paso, cuidado o pregunta frecuente).
/// </summary>
public class ServicioContenido : BaseEntity
{
    public int IdServicioMedico { get; set; }
    public string Tipo { get; set; } = TipoContenidoServicio.Introduccion;
    public string? Titulo { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int Orden { get; set; } = 0;

    public ServicioMedico? ServicioMedico { get; set; }
}
