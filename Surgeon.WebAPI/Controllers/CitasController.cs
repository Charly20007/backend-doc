using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.Domain.Common;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers;

[Route("api/[controller]")]
public class CitasController : ApiControllerBase
{
    private readonly CitasService _citas;

    public CitasController(CitasService citas) => _citas = citas;

    /// <summary>Solicitud de cita desde la web pública.</summary>
    [HttpPost]
    [AllowAnonymous]
    [EnableRateLimiting("formularios")]
    public async Task<ActionResult<CitaCreadaDto>> CrearCita([FromBody] CrearCitaDto dto)
    {
        var resultado = await _citas.SolicitarAsync(dto);
        return resultado.Exito ? StatusCode(StatusCodes.Status201Created, resultado.Valor) : Responder(resultado);
    }

    /// <summary>Agenda de citas (CMS). Contiene datos de pacientes: requiere sesión.</summary>
    [HttpGet]
    [Authorize(Roles = RolesSistema.Agenda)]
    public async Task<ActionResult<IEnumerable<CitaRespuestaDto>>> GetCitas() => Ok(await _citas.ListarAsync());

    /// <summary>Cambia el estado de una cita (Pendiente, Confirmada, Atendida, Cancelada).</summary>
    [HttpPut("{id:int}/estado")]
    [Authorize(Roles = RolesSistema.Agenda)]
    public async Task<ActionResult<EstadoCitaCambiadoDto>> CambiarEstado(int id, [FromBody] CambiarEstadoCitaDto dto) =>
        Responder(await _citas.CambiarEstadoAsync(id, dto));
}
