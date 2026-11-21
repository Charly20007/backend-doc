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
