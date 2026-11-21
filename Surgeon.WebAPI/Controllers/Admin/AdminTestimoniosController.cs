using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;
using Surgeon.Domain.Entities;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: moderación y edición de testimonios.</summary>
[Route("api/admin/testimonios")]
public class AdminTestimoniosController : AdminControllerBase
{
    private readonly IUnitOfWork _uow;

    public AdminTestimoniosController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestimonioDto>>> GetTodos()
    {
        var testimonios = await _uow.Testimonios.GetAllAsync();
        var slugs = await Slugs();
        return Ok(testimonios.OrderByDescending(t => t.FechaTestimonio).Select(t => t.ATestimonioDto(slugs)));
    }

    [HttpPost]
    public async Task<ActionResult<TestimonioDto>> Crear([FromBody] TestimonioGuardarDto dto)
    {
        if (await ServicioInvalido(dto.IdServicioMedico))
            return BadRequest(new { mensaje = "El procedimiento relacionado no existe." });

        var testimonio = new Testimonio();
        Aplicar(testimonio, dto);
        Auditar(testimonio);
        await _uow.Testimonios.AddAsync(testimonio);
        await _uow.SaveChangesAsync();
        return Ok(testimonio.ATestimonioDto(await Slugs()));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TestimonioDto>> Actualizar(int id, [FromBody] TestimonioGuardarDto dto)
    {
        var testimonio = await _uow.Testimonios.GetByIdAsync(id);
        if (testimonio == null)
            return NoEncontrado("Testimonio no encontrado");
        if (await ServicioInvalido(dto.IdServicioMedico))
            return BadRequest(new { mensaje = "El procedimiento relacionado no existe." });

        Aplicar(testimonio, dto);
        Auditar(testimonio);
        _uow.Testimonios.Update(testimonio);
        await _uow.SaveChangesAsync();
        return Ok(testimonio.ATestimonioDto(await Slugs()));
    }

    /// <summary>Aprueba u oculta un testimonio en la web sin editarlo.</summary>
    [HttpPut("{id:int}/aprobacion")]
    public async Task<ActionResult<TestimonioDto>> CambiarAprobacion(int id, [FromQuery] bool aprobado)
    {
        var testimonio = await _uow.Testimonios.GetByIdAsync(id);
        if (testimonio == null)
            return NoEncontrado("Testimonio no encontrado");

        testimonio.AprobadoParaWeb = aprobado;
        Auditar(testimonio);
        _uow.Testimonios.Update(testimonio);
        await _uow.SaveChangesAsync();
        return Ok(testimonio.ATestimonioDto(await Slugs()));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var testimonio = await _uow.Testimonios.GetByIdAsync(id);
        if (testimonio == null)
            return NoEncontrado("Testimonio no encontrado");

        Auditar(testimonio);
        _uow.Testimonios.Remove(testimonio);
        await _uow.SaveChangesAsync();
        return NoContent();
    }

    private static void Aplicar(Testimonio t, TestimonioGuardarDto dto)
    {
        t.NombrePaciente = dto.NombrePaciente.Trim();
        t.Procedimiento = dto.Procedimiento.Trim();
        t.IdServicioMedico = dto.IdServicioMedico;
        t.CalificacionEstrellas = dto.CalificacionEstrellas;
        t.Comentario = dto.Comentario.Trim();
        t.FechaTestimonio = dto.FechaTestimonio ?? (t.Id == 0 ? DateTime.UtcNow : t.FechaTestimonio);
        t.AprobadoParaWeb = dto.AprobadoParaWeb;
    }

    private async Task<bool> ServicioInvalido(int? id) =>
        id is int idServicio && await _uow.ServiciosMedicos.GetByIdAsync(idServicio) == null;

    private async Task<Dictionary<int, string>> Slugs() =>
        (await _uow.ServiciosMedicos.GetAllAsync()).ToDictionary(s => s.Id, s => s.Slug);
}
