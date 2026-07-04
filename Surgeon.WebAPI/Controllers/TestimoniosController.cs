using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;

namespace Surgeon.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestimoniosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TestimoniosController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestimonioDto>>> GetTestimonios()
    {
        var testimonios = await _unitOfWork.Testimonios.GetAllAsync();
        var webTestimonios = testimonios
            .Where(t => t.AprobadoParaWeb)
            .Select(t => new TestimonioDto
            {
                Id = t.Id,
                NombrePaciente = t.NombrePaciente,
                Procedimiento = t.Procedimiento,
                CalificacionEstrellas = t.CalificacionEstrellas,
                Comentario = t.Comentario,
                FechaTestimonio = t.FechaTestimonio
            });

        return Ok(webTestimonios);
    }
}
