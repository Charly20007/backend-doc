using Surgeon.Application.Common;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;
using Surgeon.Domain.Entities;

namespace Surgeon.Application.CasosDeUso;

/// <summary>Casos de uso de los procedimientos (servicios médicos): lectura pública y gestión desde el CMS.</summary>
public class ProcedimientosService
{
    private const string RutaBase = "/procedimientos/";
    private readonly IUnitOfWork _uow;
    private readonly RedireccionesService _redirecciones;

    public ProcedimientosService(IUnitOfWork uow, RedireccionesService redirecciones)
    {
        _uow = uow;
        _redirecciones = redirecciones;
    }

    // ---------- Lectura ----------

    public async Task<IEnumerable<ServicioMedicoDto>> ListarAsync(bool soloDestacados = false)
    {
        var servicios = await _uow.ServiciosMedicos.GetAllAsync();
        if (soloDestacados)
            servicios = servicios.Where(s => s.DestacadoEnWeb);

        var especialidades = await MapaEspecialidades();
        return servicios.OrderBy(s => s.Orden).ThenBy(s => s.Id).Select(s => s.AServicioDto(especialidades)).ToList();
    }

    public async Task<Resultado<ServicioMedicoDto>> ObtenerAsync(int id)
    {
        var servicio = await _uow.ServiciosMedicos.GetByIdAsync(id);
        if (servicio == null)
            return Error.NoEncontrado("Servicio médico no encontrado");
        return servicio.AServicioDto(await MapaEspecialidades());
    }

    public async Task<Resultado<ServicioDetalleDto>> ObtenerDetallePorSlugAsync(string slug)
    {
        var servicio = (await _uow.ServiciosMedicos.FindAsync(s => s.Slug == slug)).FirstOrDefault();
        if (servicio == null)
            return Error.NoEncontrado("Procedimiento no encontrado");
        return await Detalle(servicio);
    }

    public async Task<Resultado<ServicioDetalleDto>> ObtenerDetalleAsync(int id)
    {
        var servicio = await _uow.ServiciosMedicos.GetByIdAsync(id);
        if (servicio == null)
            return Error.NoEncontrado("Procedimiento no encontrado");
        return await Detalle(servicio);
    }

    // ---------- Gestión (CMS) ----------

    public async Task<Resultado<ServicioDetalleDto>> CrearAsync(ServicioGuardarDto dto)
    {
        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Generar(dto.Nombre) : dto.Slug;
        var error = await Validar(dto, slug, idActual: null);
        if (error != null) return error;

        var servicio = new ServicioMedico();
        Aplicar(servicio, dto, slug);
        await _uow.ServiciosMedicos.AddAsync(servicio);
        await _uow.SaveChangesAsync();

        await ReemplazarContenido(servicio.Id, dto);
        await _uow.SaveChangesAsync();
        return await Detalle(servicio);
    }

    /// <summary>Si cambia el slug, se crea automáticamente una redirección 301 desde la URL anterior.</summary>
    public async Task<Resultado<ServicioDetalleDto>> ActualizarAsync(int id, ServicioGuardarDto dto)
    {
        var servicio = await _uow.ServiciosMedicos.GetByIdAsync(id);
        if (servicio == null)
            return Error.NoEncontrado("Procedimiento no encontrado");

        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? servicio.Slug : dto.Slug;
        var error = await Validar(dto, slug, idActual: id);
        if (error != null) return error;

        var slugAnterior = servicio.Slug;
        Aplicar(servicio, dto, slug);
        _uow.ServiciosMedicos.Update(servicio);

        if (slugAnterior != slug)
            await _redirecciones.RegistrarCambioDeRutaAsync(RutaBase + slugAnterior, RutaBase + slug, $"Cambio de URL del procedimiento \"{servicio.Nombre}\"");

        await ReemplazarContenido(id, dto);
        await _uow.SaveChangesAsync();
        return await Detalle(servicio);
    }

    /// <summary>Baja lógica: la página deja de existir y las redirecciones que apuntaban a ella se desactivan.</summary>
    public async Task<Resultado> EliminarAsync(int id)
    {
        var servicio = await _uow.ServiciosMedicos.GetByIdAsync(id);
        if (servicio == null)
            return Error.NoEncontrado("Procedimiento no encontrado");

        await _redirecciones.DesactivarHaciaAsync(RutaBase + servicio.Slug);
        servicio.Slug = SlugHelper.MarcarEliminado(servicio.Slug, servicio.Id, 150);
        _uow.ServiciosMedicos.Remove(servicio);
        await _uow.SaveChangesAsync();
        return Resultado.Ok();
    }

    // ---------- Reglas ----------

    private async Task<Error?> Validar(ServicioGuardarDto dto, string slug, int? idActual)
    {
        if (string.IsNullOrEmpty(slug))
            return Error.Validacion("No se pudo generar un slug válido a partir del nombre.");

        if (await _uow.Especialidades.GetByIdAsync(dto.IdEspecialidad) == null)
            return Error.Validacion("La especialidad seleccionada no existe.");

        if ((await _uow.ServiciosMedicos.FindAsync(s => s.Slug == slug)).Any(s => s.Id != idActual))
            return Error.Conflicto($"Ya existe otro procedimiento con la URL /procedimientos/{slug}.");

        return null;
    }

    private static void Aplicar(ServicioMedico s, ServicioGuardarDto dto, string slug)
    {
        s.Nombre = dto.Nombre.Trim();
        s.NombreCorto = Mapeos.Limpiar(dto.NombreCorto);
        s.Slug = slug;
        s.Resumen = dto.Resumen.Trim();
        s.DescripcionDetallada = Mapeos.Limpiar(dto.DescripcionDetallada);
        s.TiempoEstimadoMinutos = dto.TiempoEstimadoMinutos;
        s.PrecioReferencial = dto.PrecioReferencial;
        s.Icono = Mapeos.Limpiar(dto.Icono);
        s.IdEspecialidad = dto.IdEspecialidad;
        s.DestacadoEnWeb = dto.DestacadoEnWeb;
        s.Orden = dto.Orden;
        s.CondicionTratada = Mapeos.Limpiar(dto.CondicionTratada);
        s.TituloIndicaciones = Mapeos.Limpiar(dto.TituloIndicaciones);
        s.AplicarSeo(dto.Seo);
    }

    /// <summary>El editor del CMS envía siempre el contenido completo: se reemplaza.</summary>
    private async Task ReemplazarContenido(int idServicio, ServicioGuardarDto dto)
    {
        foreach (var c in await _uow.ServiciosContenido.FindAsync(c => c.IdServicioMedico == idServicio))
            _uow.ServiciosContenido.RemovePermanently(c);

        foreach (var c in dto.AContenidos(idServicio))
            await _uow.ServiciosContenido.AddAsync(c);
    }

    private async Task<ServicioDetalleDto> Detalle(ServicioMedico servicio)
    {
        var contenidos = await _uow.ServiciosContenido.FindAsync(c => c.IdServicioMedico == servicio.Id);
        return servicio.AServicioDetalleDto(await MapaEspecialidades(), contenidos);
    }

    private async Task<Dictionary<int, string>> MapaEspecialidades() =>
        (await _uow.Especialidades.GetAllAsync()).ToDictionary(e => e.Id, e => e.Nombre);
}
