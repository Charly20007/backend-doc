using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;

namespace Surgeon.WebAPI.Controllers;

/// <summary>Procedimientos publicados en la web (lectura pública).</summary>
[ApiController]
[Route("api/[controller]")]
public class ServiciosController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public ServiciosController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServicioMedicoDto>>> GetServicios([FromQuery] bool soloDestacados = false)
    {
        var servicios = await _uow.ServiciosMedicos.GetAllAsync();
        if (soloDestacados)
            servicios = servicios.Where(s => s.DestacadoEnWeb);

        var especialidades = (await _uow.Especialidades.GetAllAsync()).ToDictionary(e => e.Id, e => e.Nombre);
        return Ok(servicios.OrderBy(s => s.Orden).ThenBy(s => s.Id).Select(s => s.AServicioDto(especialidades)));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServicioMedicoDto>> GetServicioById(int id)
    {
        var servicio = await _uow.ServiciosMedicos.GetByIdAsync(id);
        if (servicio == null)
            return NotFound(new { mensaje = "Servicio médico no encontrado" });

        var especialidades = (await _uow.Especialidades.GetAllAsync()).ToDictionary(e => e.Id, e => e.Nombre);
        return Ok(servicio.AServicioDto(especialidades));
    }

    /// <summary>Detalle completo (contenido + SEO) para la página /procedimientos/{slug}.</summary>
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ServicioDetalleDto>> GetServicioBySlug(string slug)
    {
        var servicio = (await _uow.ServiciosMedicos.FindAsync(s => s.Slug == slug)).FirstOrDefault();
        if (servicio == null)
            return NotFound(new { mensaje = "Procedimiento no encontrado" });

        var especialidades = (await _uow.Especialidades.GetAllAsync()).ToDictionary(e => e.Id, e => e.Nombre);
        var contenidos = await _uow.ServiciosContenido.FindAsync(c => c.IdServicioMedico == servicio.Id);
        return Ok(servicio.AServicioDetalleDto(especialidades, contenidos));
    }
}
