using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

public class Testimonio : BaseEntity
{
    public string NombrePaciente { get; set; } = string.Empty;
    public string Procedimiento { get; set; } = string.Empty;
    /// <summary>Procedimiento relacionado: el testimonio también se muestra en su página.</summary>
    public int? IdServicioMedico { get; set; }
    public int CalificacionEstrellas { get; set; } = 5;
    public string Comentario { get; set; } = string.Empty;
    public DateTime FechaTestimonio { get; set; } = DateTime.UtcNow;
    public bool AprobadoParaWeb { get; set; } = true;

    /// <summary>Lo envió un paciente desde la web (no lo cargó el administrador).</summary>
    public bool EnviadoDesdeWeb { get; set; }
    /// <summary>Enviado desde la web y aún sin revisar: el CMS lo muestra en "Por revisar".</summary>
    public bool PendienteRevision { get; set; }
    /// <summary>El paciente marcó que autoriza publicar su testimonio con su nombre (Ley 29733).</summary>
    public bool AutorizaPublicacion { get; set; }
    /// <summary>Teléfono o correo para verificar que es un paciente real. Nunca se publica.</summary>
    public string? ContactoPaciente { get; set; }

    public ServicioMedico? ServicioMedico { get; set; }
}
