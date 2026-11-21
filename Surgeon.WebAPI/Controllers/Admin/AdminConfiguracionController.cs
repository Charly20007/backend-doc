using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: datos del doctor, contacto, horarios y SEO global.</summary>
[Route("api/admin/configuracion")]
public class AdminConfiguracionController : AdminControllerBase
{
    private readonly ConfiguracionService _configuracion;

    public AdminConfiguracionController(ConfiguracionService configuracion) => _configuracion = configuracion;

    [HttpGet]
    public async Task<ActionResult<ConfiguracionSitioDto>> Get() => Ok(await _configuracion.ObtenerParaEditarAsync());

    [HttpPut]
    public async Task<ActionResult<ConfiguracionSitioDto>> Actualizar([FromBody] ConfiguracionSitioDto dto) =>
        Ok(await _configuracion.ActualizarAsync(dto));

    /// <summary>Reemplaza los horarios de atención (se envían todos los tramos).</summary>
    [HttpPut("horarios")]
    public async Task<ActionResult<List<HorarioDto>>> ActualizarHorarios([FromBody] List<HorarioDto> horarios) =>
        Responder(await _configuracion.ActualizarHorariosAsync(horarios));
}
