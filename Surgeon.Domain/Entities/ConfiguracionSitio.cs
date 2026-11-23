using Surgeon.Domain.Common;

namespace Surgeon.Domain.Entities;

/// <summary>
/// Datos globales del sitio (una sola fila): datos del doctor, contacto y SEO por defecto.
/// Se editan desde el CMS y la web los usa en navbar, footer, Schema.org y metadatos.
/// </summary>
public class ConfiguracionSitio : BaseEntity
{
    // Doctor
    public string NombreDoctor { get; set; } = string.Empty;
    public string? NombreCorto { get; set; }
    public string? Especialidad { get; set; }
    public string? Cmp { get; set; }
    public string? Rne { get; set; }
    public string? FotoDoctorUrl { get; set; }
    public string? SegurosAliados { get; set; }

    // Contacto
    public string? TelefonoPrincipal { get; set; }
    public string? TelefonoSecundario { get; set; }
    public string? WhatsApp { get; set; }
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
    public string? Distrito { get; set; }
    public string? Ciudad { get; set; }
    public string? Region { get; set; }
    public string Pais { get; set; } = "PE";
    public string? GoogleMapsUrl { get; set; }

    // Pie de página
    /// <summary>Frase corta bajo el nombre del doctor en el pie de página.</summary>
    public string? DescripcionPie { get; set; }
    /// <summary>Muestra la línea de emergencias en el pie de página y en contacto.</summary>
    public bool MostrarEmergencias { get; set; } = false;
    /// <summary>Ej.: "Emergencias quirúrgicas: 24 horas".</summary>
    public string? TextoEmergencias { get; set; }

    // Reservas online
    /// <summary>Si es false, el formulario de la web no ofrece turnos y pide llamar.</summary>
    public bool ReservasOnlineActivas { get; set; } = true;
    /// <summary>Duración de cada turno de consulta.</summary>
    public int DuracionTurnoMinutos { get; set; } = 30;
    /// <summary>No se puede reservar un turno que empieza antes de estas horas desde ahora.</summary>
    public int AnticipacionMinimaHoras { get; set; } = 2;
    /// <summary>Hasta cuántos días hacia adelante se muestran turnos.</summary>
    public int DiasMaximosReserva { get; set; } = 30;

    // Reserva en una clínica externa (ej.: SANNA)
    /// <summary>Si es true, «Agendar cita» ofrece también reservar en la clínica.</summary>
    public bool ReservaClinicaActiva { get; set; } = false;
    /// <summary>Ej.: "Clínica SANNA San Borja".</summary>
    public string? ReservaClinicaNombre { get; set; }
    /// <summary>Página de la clínica donde se reserva (puede llevar #:~:text= para resaltar el nombre).</summary>
    public string? ReservaClinicaUrl { get; set; }
    /// <summary>Central telefónica de la clínica.</summary>
    public string? ReservaClinicaTelefono { get; set; }
    /// <summary>Texto que el paciente debe buscar en la web de la clínica. Ej.: "Marcos Enriquez".</summary>
    public string? ReservaClinicaTextoBusqueda { get; set; }

    // Redes sociales
    public string? FacebookUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? TikTokUrl { get; set; }
    public string? YouTubeUrl { get; set; }
    public string? LinkedInUrl { get; set; }

    // SEO global
    public string SiteUrl { get; set; } = string.Empty;
    public string? MetaTituloPorDefecto { get; set; }
    public string? MetaDescripcionPorDefecto { get; set; }
    public string? ImagenOgPorDefectoUrl { get; set; }
    /// <summary>Código de verificación de Google Search Console (meta google-site-verification).</summary>
    public string? GoogleSiteVerification { get; set; }
    /// <summary>ID de Google Analytics 4 (G-XXXXXXX).</summary>
    public string? GoogleAnalyticsId { get; set; }
}
