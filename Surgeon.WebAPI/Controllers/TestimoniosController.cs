using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers;

/// <summary>Testimonios aprobados para la web (lectura pública).</summary>
[Route("api/[controller]")]
public class TestimoniosController : ApiControllerBase
{
    private readonly TestimoniosService _testimonios;

    public TestimoniosController(TestimoniosService testimonios) => _testimonios = testimonios;

    /// <param name="servicio">Slug del procedimiento para filtrar (opcional).</param>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestimonioDto>>> GetTestimonios([FromQuery] string? servicio = null) =>
        Ok(await _testimonios.ListarAprobadosAsync(servicio));
}
