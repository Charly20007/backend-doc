using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class ServicioMedico : BaseEntity, ISeoEntity
{
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Nombre corto para menús y migas de pan (ej. "Cirugía de Vesícula").</summary>
    public string? NombreCorto { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Resumen { get; set; } = string.Empty;
    public string? DescripcionDetallada { get; set; }
    public int TiempoEstimadoMinutos { get; set; } = 45;
    public decimal? PrecioReferencial { get; set; }
    public string? Icono { get; set; }
    public int IdEspecialidad { get; set; }
    public bool DestacadoEnWeb { get; set; } = true;
    /// <summary>Orden de aparición en la web.</summary>
    public int Orden { get; set; } = 0;

    /// <summary>Condición tratada (Schema.org MedicalCondition), ej. "Apendicitis aguda".</summary>
    public string? CondicionTratada { get; set; }
    /// <summary>Encabezado de la lista de indicaciones, ej. "Síntomas de apendicitis".</summary>
    public string? TituloIndicaciones { get; set; }

    // SEO
    public string? MetaTitulo { get; set; }
    public string? MetaDescripcion { get; set; }
    public string? ImagenOgUrl { get; set; }
    public bool Indexable { get; set; } = true;

    public Especialidad? Especialidad { get; set; }
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    public ICollection<ServicioContenido> Contenidos { get; set; } = new List<ServicioContenido>();
}
