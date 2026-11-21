using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers;

/// <summary>Procedimientos publicados en la web (lectura pública).</summary>
[Route("api/[controller]")]
public class ServiciosController : ApiControllerBase
{
    private readonly ProcedimientosService _procedimientos;

    public ServiciosController(ProcedimientosService procedimientos) => _procedimientos = procedimientos;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServicioMedicoDto>>> GetServicios([FromQuery] bool soloDestacados = false) =>
        Ok(await _procedimientos.ListarAsync(soloDestacados));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServicioMedicoDto>> GetServicioById(int id) =>
        Responder(await _procedimientos.ObtenerAsync(id));

    /// <summary>Detalle completo (contenido + SEO) para la página /procedimientos/{slug}.</summary>
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ServicioDetalleDto>> GetServicioBySlug(string slug) =>
        Responder(await _procedimientos.ObtenerDetallePorSlugAsync(slug));
}
