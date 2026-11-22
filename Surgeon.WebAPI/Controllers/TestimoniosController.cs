using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers;

/// <summary>Testimonios de la web: lectura de los aprobados y envío de nuevos por pacientes.</summary>
[Route("api/[controller]")]
public class TestimoniosController : ApiControllerBase
{
    private readonly TestimoniosService _testimonios;

    public TestimoniosController(TestimoniosService testimonios) => _testimonios = testimonios;

    /// <param name="servicio">Slug del procedimiento para filtrar (opcional).</param>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestimonioDto>>> GetTestimonios([FromQuery] string? servicio = null) =>
        Ok(await _testimonios.ListarAprobadosAsync(servicio));

    /// <summary>Un paciente envía su testimonio. Queda pendiente de aprobación en el CMS (202).</summary>
    [HttpPost]
    [AllowAnonymous]
    [EnableRateLimiting("formularios")]
    public async Task<ActionResult> Enviar([FromBody] TestimonioEnviarDto dto)
    {
        var resultado = await _testimonios.EnviarDesdeWebAsync(dto);
        return resultado.Exito
            ? Accepted(new { mensaje = "Gracias. Tu testimonio se publicará cuando el consultorio lo revise." })
            : SinContenido(resultado);
    }
}
