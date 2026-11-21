using System.Text.RegularExpressions;
using Surgeon.Application.DTOs;
using Surgeon.Domain.Common;
using Surgeon.Domain.Entities;

namespace Surgeon.Application.Mapping;

public static partial class Mapeos
{
    public static SeoDto ASeoDto(this ISeoEntity e) => new()
    {
        MetaTitulo = e.MetaTitulo,
        MetaDescripcion = e.MetaDescripcion,
        ImagenOgUrl = e.ImagenOgUrl,
        Indexable = e.Indexable,
    };

    public static void AplicarSeo(this ISeoEntity e, SeoDto seo)
    {
        e.MetaTitulo = Limpiar(seo.MetaTitulo);
        e.MetaDescripcion = Limpiar(seo.MetaDescripcion);
        e.ImagenOgUrl = Limpiar(seo.ImagenOgUrl);
        e.Indexable = seo.Indexable;
    }

    /// <summary>Convierte cadenas vacías en null y quita espacios sobrantes.</summary>
    public static string? Limpiar(string? valor) => string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();

    public static ServicioMedicoDto AServicioDto(this ServicioMedico s, IReadOnlyDictionary<int, string> especialidades) =>
        Llenar(new ServicioMedicoDto(), s, especialidades);

    public static ServicioDetalleDto AServicioDetalleDto(this ServicioMedico s, IReadOnlyDictionary<int, string> especialidades, IEnumerable<ServicioContenido> contenidos)
    {
        var dto = Llenar(new ServicioDetalleDto(), s, especialidades);
        dto.CondicionTratada = s.CondicionTratada;
        dto.TituloIndicaciones = s.TituloIndicaciones;
        dto.Seo = s.ASeoDto();

        var porTipo = contenidos.OrderBy(c => c.Orden).ThenBy(c => c.Id).ToLookup(c => c.Tipo);
        List<ContenidoItemDto> Items(string tipo) => porTipo[tipo].Select(c => new ContenidoItemDto { Titulo = c.Titulo, Texto = c.Texto }).ToList();

        dto.Introduccion = Items(TipoContenidoServicio.Introduccion);
        dto.Indicaciones = Items(TipoContenidoServicio.Indicacion);
        dto.Pasos = Items(TipoContenidoServicio.Paso);
        dto.Recuperacion = Items(TipoContenidoServicio.Recuperacion);
        dto.Preguntas = Items(TipoContenidoServicio.Pregunta);
        return dto;
    }

    private static T Llenar<T>(T dto, ServicioMedico s, IReadOnlyDictionary<int, string> especialidades) where T : ServicioMedicoDto
    {
        dto.Id = s.Id;
        dto.Nombre = s.Nombre;
        dto.NombreCorto = s.NombreCorto;
        dto.Slug = s.Slug;
        dto.Resumen = s.Resumen;
        dto.DescripcionDetallada = s.DescripcionDetallada;
        dto.TiempoEstimadoMinutos = s.TiempoEstimadoMinutos;
        dto.PrecioReferencial = s.PrecioReferencial;
        dto.Icono = s.Icono;
        dto.IdEspecialidad = s.IdEspecialidad;
        dto.EspecialidadNombre = especialidades.TryGetValue(s.IdEspecialidad, out var nombre) ? nombre : "General";
        dto.DestacadoEnWeb = s.DestacadoEnWeb;
        dto.Orden = s.Orden;
        dto.Indexable = s.Indexable;
        dto.FechaModificacion = s.FechaModificacion;
        return dto;
    }

    /// <summary>Convierte las listas del DTO en filas de ServiciosContenido.</summary>
    public static IEnumerable<ServicioContenido> AContenidos(this ServicioGuardarDto dto, int idServicio)
    {
        IEnumerable<ServicioContenido> Bloques(string tipo, List<ContenidoItemDto> items) =>
            items.Where(i => !string.IsNullOrWhiteSpace(i.Texto)).Select((i, idx) => new ServicioContenido
            {
                IdServicioMedico = idServicio,
                Tipo = tipo,
                Titulo = Limpiar(i.Titulo),
                Texto = i.Texto.Trim(),
                Orden = idx + 1,
            });

        return Bloques(TipoContenidoServicio.Introduccion, dto.Introduccion)
            .Concat(Bloques(TipoContenidoServicio.Indicacion, dto.Indicaciones))
            .Concat(Bloques(TipoContenidoServicio.Paso, dto.Pasos))
            .Concat(Bloques(TipoContenidoServicio.Recuperacion, dto.Recuperacion))
            .Concat(Bloques(TipoContenidoServicio.Pregunta, dto.Preguntas));
    }

    public static ArticuloBlogDto AArticuloDto(this ArticuloBlog a, CategoriaBlog? categoria, ServicioMedico? servicio) => new()
    {
        Id = a.Id,
        Titulo = a.Titulo,
        Slug = a.Slug,
        Resumen = a.Resumen,
        ContenidoHtml = a.ContenidoHtml,
        ImagenPortadaUrl = a.ImagenPortadaUrl,
        Categoria = categoria == null
            ? new CategoriaBlogDto { Id = a.IdCategoria, Nombre = "General", Slug = "general" }
            : new CategoriaBlogDto { Id = categoria.Id, Nombre = categoria.Nombre, Slug = categoria.Slug },
        ServicioRelacionado = servicio == null ? null : new ServicioRelacionadoDto { Id = servicio.Id, Nombre = servicio.Nombre, Slug = servicio.Slug },
        FechaPublicacion = a.FechaPublicacion,
        FechaModificacion = a.FechaModificacion,
        Autor = a.Autor,
        TiempoLectura = a.TiempoLecturaMinutos,
        Vistas = a.Vistas,
        PublicadoEnWeb = a.PublicadoEnWeb,
        Seo = a.ASeoDto(),
    };

    /// <summary>Minutos de lectura estimados a ~200 palabras por minuto (ignora etiquetas HTML).</summary>
    public static int CalcularTiempoLectura(string html)
    {
        var texto = EtiquetasHtml().Replace(html, " ");
        var palabras = texto.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
        return Math.Max(1, (int)Math.Ceiling(palabras / 200.0));
    }

    public static TestimonioDto ATestimonioDto(this Testimonio t, IReadOnlyDictionary<int, string> slugsServicios) => new()
    {
        Id = t.Id,
        NombrePaciente = t.NombrePaciente,
        Procedimiento = t.Procedimiento,
        IdServicioMedico = t.IdServicioMedico,
        ProcedimientoSlug = t.IdServicioMedico is int id && slugsServicios.TryGetValue(id, out var slug) ? slug : null,
        CalificacionEstrellas = t.CalificacionEstrellas,
        Comentario = t.Comentario,
        FechaTestimonio = t.FechaTestimonio,
        AprobadoParaWeb = t.AprobadoParaWeb,
    };

    public static PaginaSeoDto APaginaSeoDto(this PaginaSeo p) => new()
    {
        Id = p.Id,
        Clave = p.Clave,
        Ruta = p.Ruta,
        NombrePagina = p.NombrePagina,
        Seo = p.ASeoDto(),
        FechaModificacion = p.FechaModificacion,
    };

    public static RedireccionDto ARedireccionDto(this Redireccion r) => new()
    {
        Id = r.Id,
        RutaOrigen = r.RutaOrigen,
        RutaDestino = r.RutaDestino,
        EsPermanente = r.EsPermanente,
        Activa = r.Activa,
        Nota = r.Nota,
    };

    public static ConfiguracionSitioDto AConfiguracionDto(this ConfiguracionSitio c, IEnumerable<HorarioAtencion> horarios) => new()
    {
        NombreDoctor = c.NombreDoctor,
        NombreCorto = c.NombreCorto,
        Especialidad = c.Especialidad,
        Cmp = c.Cmp,
        Rne = c.Rne,
        FotoDoctorUrl = c.FotoDoctorUrl,
        SegurosAliados = c.SegurosAliados,
        TelefonoPrincipal = c.TelefonoPrincipal,
        TelefonoSecundario = c.TelefonoSecundario,
        WhatsApp = c.WhatsApp,
        Correo = c.Correo,
        Direccion = c.Direccion,
        Distrito = c.Distrito,
        Ciudad = c.Ciudad,
        Region = c.Region,
        Pais = c.Pais,
        GoogleMapsUrl = c.GoogleMapsUrl,
        FacebookUrl = c.FacebookUrl,
        InstagramUrl = c.InstagramUrl,
        TikTokUrl = c.TikTokUrl,
        YouTubeUrl = c.YouTubeUrl,
        LinkedInUrl = c.LinkedInUrl,
        SiteUrl = c.SiteUrl,
        MetaTituloPorDefecto = c.MetaTituloPorDefecto,
        MetaDescripcionPorDefecto = c.MetaDescripcionPorDefecto,
        ImagenOgPorDefectoUrl = c.ImagenOgPorDefectoUrl,
        GoogleSiteVerification = c.GoogleSiteVerification,
        GoogleAnalyticsId = c.GoogleAnalyticsId,
        DescripcionPie = c.DescripcionPie,
        MostrarEmergencias = c.MostrarEmergencias,
        TextoEmergencias = c.TextoEmergencias,
        Horarios = horarios
            .Where(h => h.EsActivo)
            .OrderBy(h => h.DiaSemana).ThenBy(h => h.HoraInicio)
            .Select(h => new HorarioDto { DiaSemana = h.DiaSemana, HoraInicio = h.HoraInicio.ToString(@"hh\:mm"), HoraFin = h.HoraFin.ToString(@"hh\:mm") })
            .ToList(),
    };

    public static void AplicarConfiguracion(this ConfiguracionSitio c, ConfiguracionSitioDto dto)
    {
        c.NombreDoctor = dto.NombreDoctor.Trim();
        c.NombreCorto = Limpiar(dto.NombreCorto);
        c.Especialidad = Limpiar(dto.Especialidad);
        c.Cmp = Limpiar(dto.Cmp);
        c.Rne = Limpiar(dto.Rne);
        c.FotoDoctorUrl = Limpiar(dto.FotoDoctorUrl);
        c.SegurosAliados = Limpiar(dto.SegurosAliados);
        c.TelefonoPrincipal = Limpiar(dto.TelefonoPrincipal);
        c.TelefonoSecundario = Limpiar(dto.TelefonoSecundario);
        c.WhatsApp = Limpiar(dto.WhatsApp);
        c.Correo = Limpiar(dto.Correo);
        c.Direccion = Limpiar(dto.Direccion);
        c.Distrito = Limpiar(dto.Distrito);
        c.Ciudad = Limpiar(dto.Ciudad);
        c.Region = Limpiar(dto.Region);
        c.Pais = dto.Pais.Trim().ToUpperInvariant();
        c.GoogleMapsUrl = Limpiar(dto.GoogleMapsUrl);
        c.FacebookUrl = Limpiar(dto.FacebookUrl);
        c.InstagramUrl = Limpiar(dto.InstagramUrl);
        c.TikTokUrl = Limpiar(dto.TikTokUrl);
        c.YouTubeUrl = Limpiar(dto.YouTubeUrl);
        c.LinkedInUrl = Limpiar(dto.LinkedInUrl);
        c.SiteUrl = dto.SiteUrl.Trim().TrimEnd('/');
        c.MetaTituloPorDefecto = Limpiar(dto.MetaTituloPorDefecto);
        c.MetaDescripcionPorDefecto = Limpiar(dto.MetaDescripcionPorDefecto);
        c.ImagenOgPorDefectoUrl = Limpiar(dto.ImagenOgPorDefectoUrl);
        c.GoogleSiteVerification = Limpiar(dto.GoogleSiteVerification);
        c.GoogleAnalyticsId = Limpiar(dto.GoogleAnalyticsId);
        c.DescripcionPie = Limpiar(dto.DescripcionPie);
        c.MostrarEmergencias = dto.MostrarEmergencias;
        c.TextoEmergencias = Limpiar(dto.TextoEmergencias);
    }

    public static PaginaLegalResumenDto APaginaLegalResumenDto(this PaginaLegal p) => new()
    {
        Clave = p.Clave,
        Ruta = p.Ruta,
        Titulo = p.Titulo,
        Publicada = p.Publicada,
        FechaModificacion = p.FechaModificacion,
    };

    public static PaginaLegalDto APaginaLegalDto(this PaginaLegal p) => new()
    {
        Clave = p.Clave,
        Ruta = p.Ruta,
        Titulo = p.Titulo,
        Publicada = p.Publicada,
        FechaModificacion = p.FechaModificacion,
        ContenidoHtml = p.ContenidoHtml,
        Seo = p.ASeoDto(),
    };

    [GeneratedRegex("<[^>]+>")]
    private static partial Regex EtiquetasHtml();
}
