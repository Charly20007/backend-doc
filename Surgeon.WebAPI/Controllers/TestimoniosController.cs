using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;

namespace Surgeon.WebAPI.Controllers;

/// <summary>Testimonios aprobados para la web (lectura pública).</summary>
[ApiController]
[Route("api/[controller]")]
public class TestimoniosController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public TestimoniosController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    /// <param name="servicio">Slug del procedimiento para filtrar (opcional).</param>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestimonioDto>>> GetTestimonios([FromQuery] string? servicio = null)
    {
        var testimonios = await _uow.Testimonios.FindAsync(t => t.AprobadoParaWeb);
        var slugs = (await _uow.ServiciosMedicos.GetAllAsync()).ToDictionary(s => s.Id, s => s.Slug);

        var dtos = testimonios
            .OrderByDescending(t => t.FechaTestimonio)
            .Select(t => t.ATestimonioDto(slugs));

        if (!string.IsNullOrWhiteSpace(servicio))
            dtos = dtos.Where(t => t.ProcedimientoSlug == servicio);

        return Ok(dtos);
    }
}
