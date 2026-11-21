using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.Common;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;
using Surgeon.Application.Services;
using Surgeon.Domain.Entities;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: alta, edición (contenido + SEO) y baja de procedimientos.</summary>
[Route("api/admin/servicios")]
public class AdminServiciosController : AdminControllerBase
{
    private const string RutaBase = "/procedimientos/";
    private readonly IUnitOfWork _uow;
    private readonly RedireccionesService _redirecciones;

    public AdminServiciosController(IUnitOfWork uow, RedireccionesService redirecciones)
    {
        _uow = uow;
        _redirecciones = redirecciones;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServicioMedicoDto>>> GetTodos()
    {
        var especialidades = await MapaEspecialidades();
        var servicios = await _uow.ServiciosMedicos.GetAllAsync();
        return Ok(servicios.OrderBy(s => s.Orden).ThenBy(s => s.Id).Select(s => s.AServicioDto(especialidades)));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServicioDetalleDto>> GetDetalle(int id)
    {
        var servicio = await _uow.ServiciosMedicos.GetByIdAsync(id);
        if (servicio == null)
            return NoEncontrado("Procedimiento no encontrado");

        var contenidos = await _uow.ServiciosContenido.FindAsync(c => c.IdServicioMedico == id);
        return Ok(servicio.AServicioDetalleDto(await MapaEspecialidades(), contenidos));
    }

    [HttpPost]
    public async Task<ActionResult<ServicioDetalleDto>> Crear([FromBody] ServicioGuardarDto dto)
    {
        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Generar(dto.Nombre) : dto.Slug;
        var error = await Validar(dto, slug, idActual: null);
        if (error != null) return error;

        var servicio = new ServicioMedico();
        Aplicar(servicio, dto, slug);
        Auditar(servicio);
        await _uow.ServiciosMedicos.AddAsync(servicio);
        await _uow.SaveChangesAsync();

        await ReemplazarContenido(servicio.Id, dto);
        await _uow.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDetalle), new { id = servicio.Id }, await Detalle(servicio));
    }

    /// <summary>
    /// Actualiza todo el procedimiento. Si cambia el slug, se crea automáticamente
    /// una redirección 301 desde la URL anterior.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ServicioDetalleDto>> Actualizar(int id, [FromBody] ServicioGuardarDto dto)
    {
        var servicio = await _uow.ServiciosMedicos.GetByIdAsync(id);
        if (servicio == null)
            return NoEncontrado("Procedimiento no encontrado");

        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? servicio.Slug : dto.Slug;
        var error = await Validar(dto, slug, idActual: id);
        if (error != null) return error;

        var slugAnterior = servicio.Slug;
        Aplicar(servicio, dto, slug);
        Auditar(servicio);
        _uow.ServiciosMedicos.Update(servicio);

        if (slugAnterior != slug)
            await _redirecciones.RegistrarCambioDeRutaAsync(RutaBase + slugAnterior, RutaBase + slug, $"Cambio de URL del procedimiento \"{servicio.Nombre}\"");

        await ReemplazarContenido(id, dto);
        await _uow.SaveChangesAsync();

        return Ok(await Detalle(servicio));
    }

    /// <summary>Baja lógica. La URL deja de existir (404); si quiere conservar el tráfico, cree una redirección.</summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var servicio = await _uow.ServiciosMedicos.GetByIdAsync(id);
        if (servicio == null)
            return NoEncontrado("Procedimiento no encontrado");

        // Libera el slug (la columna es UNIQUE aunque el registro esté eliminado)
        await _redirecciones.DesactivarHaciaAsync(RutaBase + servicio.Slug);

        var sufijo = $"--eliminado-{servicio.Id}";
        servicio.Slug = (servicio.Slug.Length + sufijo.Length > 150 ? servicio.Slug[..(150 - sufijo.Length)] : servicio.Slug) + sufijo;
        Auditar(servicio);
        _uow.ServiciosMedicos.Remove(servicio);
        await _uow.SaveChangesAsync();
        return NoContent();
    }

    private async Task<ActionResult?> Validar(ServicioGuardarDto dto, string slug, int? idActual)
    {
        if (string.IsNullOrEmpty(slug))
            return BadRequest(new { mensaje = "No se pudo generar un slug válido a partir del nombre." });

        if (await _uow.Especialidades.GetByIdAsync(dto.IdEspecialidad) == null)
            return BadRequest(new { mensaje = "La especialidad seleccionada no existe." });

        var duplicado = (await _uow.ServiciosMedicos.FindAsync(s => s.Slug == slug)).Any(s => s.Id != idActual);
        if (duplicado)
            return Conflicto($"Ya existe otro procedimiento con la URL /procedimientos/{slug}.");

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
        {
            Auditar(c);
            await _uow.ServiciosContenido.AddAsync(c);
        }
    }

    private async Task<ServicioDetalleDto> Detalle(ServicioMedico servicio)
    {
        var contenidos = await _uow.ServiciosContenido.FindAsync(c => c.IdServicioMedico == servicio.Id);
        return servicio.AServicioDetalleDto(await MapaEspecialidades(), contenidos);
    }

    private async Task<Dictionary<int, string>> MapaEspecialidades() =>
        (await _uow.Especialidades.GetAllAsync()).ToDictionary(e => e.Id, e => e.Nombre);
}
