using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers;

/// <summary>Turnos libres para el formulario de citas (público: solo dice libre/ocupado, nunca quién).</summary>
[Route("api/agenda")]
public class AgendaController : ApiControllerBase
{
    private readonly AgendaService _agenda;

    public AgendaController(AgendaService agenda) => _agenda = agenda;

    [HttpGet("disponibilidad")]
    [ResponseCache(NoStore = true)]
    public async Task<ActionResult<DisponibilidadDto>> GetDisponibilidad() => Ok(await _agenda.ObtenerDisponibilidadAsync());
}
