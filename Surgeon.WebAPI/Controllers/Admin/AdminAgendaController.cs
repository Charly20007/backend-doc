using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.Domain.Common;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: bloqueos de agenda (vacaciones, congresos, días de quirófano). También para la secretaria.</summary>
[Route("api/admin/agenda")]
[Authorize(Roles = RolesSistema.Agenda)]
public class AdminAgendaController : ApiControllerBase
{
    private readonly AgendaService _agenda;

    public AdminAgendaController(AgendaService agenda) => _agenda = agenda;

    [HttpGet("bloqueos")]
    public async Task<ActionResult<IEnumerable<BloqueoDto>>> GetBloqueos() => Ok(await _agenda.ListarBloqueosAsync());

    [HttpPost("bloqueos")]
    public async Task<ActionResult<BloqueoDto>> CrearBloqueo([FromBody] BloqueoGuardarDto dto) => Responder(await _agenda.CrearBloqueoAsync(dto));

    [HttpDelete("bloqueos/{id:int}")]
    public async Task<ActionResult> EliminarBloqueo(int id) => SinContenido(await _agenda.EliminarBloqueoAsync(id));
}
