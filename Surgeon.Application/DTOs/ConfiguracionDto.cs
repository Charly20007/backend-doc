using System.ComponentModel.DataAnnotations;

namespace Surgeon.Application.DTOs;

public class ConfiguracionSitioDto
{
    [Required(ErrorMessage = "El nombre del doctor es obligatorio.")]
    [MaxLength(120)] public string NombreDoctor { get; set; } = string.Empty;
    [MaxLength(60)] public string? NombreCorto { get; set; }
    [MaxLength(150)] public string? Especialidad { get; set; }
    [MaxLength(20)] public string? Cmp { get; set; }
    [MaxLength(20)] public string? Rne { get; set; }
    [MaxLength(500)] public string? FotoDoctorUrl { get; set; }
    [MaxLength(200)] public string? SegurosAliados { get; set; }

    [MaxLength(30)][RegularExpression(@"^\+?[0-9 ()-]{6,30}$", ErrorMessage = "Teléfono no válido.")]
    public string? TelefonoPrincipal { get; set; }
    [MaxLength(30)][RegularExpression(@"^\+?[0-9 ()-]{6,30}$", ErrorMessage = "Teléfono no válido.")]
    public string? TelefonoSecundario { get; set; }
    [MaxLength(30)][RegularExpression(@"^\+?[0-9 ()-]{6,30}$", ErrorMessage = "WhatsApp no válido.")]
    public string? WhatsApp { get; set; }
    [MaxLength(100)][EmailAddress(ErrorMessage = "Correo no válido.")]
    public string? Correo { get; set; }
    [MaxLength(200)] public string? Direccion { get; set; }
    [MaxLength(80)] public string? Distrito { get; set; }
    [MaxLength(80)] public string? Ciudad { get; set; }
    [MaxLength(80)] public string? Region { get; set; }
    [Required][StringLength(2, MinimumLength = 2)] public string Pais { get; set; } = "PE";
    [MaxLength(1000)][Url] public string? GoogleMapsUrl { get; set; }

    [MaxLength(300)] public string? DescripcionPie { get; set; }
    public bool MostrarEmergencias { get; set; }
    [MaxLength(100)] public string? TextoEmergencias { get; set; }

    [MaxLength(300)][Url] public string? FacebookUrl { get; set; }
    [MaxLength(300)][Url] public string? InstagramUrl { get; set; }
    [MaxLength(300)][Url] public string? TikTokUrl { get; set; }
    [MaxLength(300)][Url] public string? YouTubeUrl { get; set; }
    [MaxLength(300)][Url] public string? LinkedInUrl { get; set; }

    [Required(ErrorMessage = "La URL del sitio es obligatoria.")]
    [MaxLength(200)][Url(ErrorMessage = "La URL del sitio no es válida.")]
    public string SiteUrl { get; set; } = string.Empty;
    [MaxLength(70)] public string? MetaTituloPorDefecto { get; set; }
    [MaxLength(170)] public string? MetaDescripcionPorDefecto { get; set; }
    [MaxLength(500)] public string? ImagenOgPorDefectoUrl { get; set; }
    [MaxLength(100)] public string? GoogleSiteVerification { get; set; }
    [MaxLength(30)][RegularExpression(@"^G-[A-Z0-9]+$", ErrorMessage = "El ID de Google Analytics debe tener la forma G-XXXXXXX.")]
    public string? GoogleAnalyticsId { get; set; }

    /// <summary>Solo lectura: se edita con el endpoint de horarios.</summary>
    public List<HorarioDto> Horarios { get; set; } = new();
}

public class HorarioDto
{
    /// <summary>1 = Lunes ... 7 = Domingo.</summary>
    [Range(1, 7, ErrorMessage = "El día debe estar entre 1 (lunes) y 7 (domingo).")]
    public int DiaSemana { get; set; }

    [Required][RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Hora no válida (HH:mm).")]
    public string HoraInicio { get; set; } = "09:00";

    [Required][RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Hora no válida (HH:mm).")]
    public string HoraFin { get; set; } = "13:00";
}
