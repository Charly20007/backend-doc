using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: moderación y edición de testimonios.</summary>
[Route("api/admin/testimonios")]
public class AdminTestimoniosController : AdminControllerBase
{
    private readonly TestimoniosService _testimonios;

    public AdminTestimoniosController(TestimoniosService testimonios) => _testimonios = testimonios;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestimonioDto>>> GetTodos() => Ok(await _testimonios.ListarTodosAsync());

    [HttpPost]
    public async Task<ActionResult<TestimonioDto>> Crear([FromBody] TestimonioGuardarDto dto) => Responder(await _testimonios.CrearAsync(dto));

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TestimonioDto>> Actualizar(int id, [FromBody] TestimonioGuardarDto dto) =>
        Responder(await _testimonios.ActualizarAsync(id, dto));

    /// <summary>Aprueba u oculta un testimonio en la web sin editarlo.</summary>
    [HttpPut("{id:int}/aprobacion")]
    public async Task<ActionResult<TestimonioDto>> CambiarAprobacion(int id, [FromQuery] bool aprobado) =>
        Responder(await _testimonios.CambiarAprobacionAsync(id, aprobado));

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Eliminar(int id) => SinContenido(await _testimonios.EliminarAsync(id));
}
