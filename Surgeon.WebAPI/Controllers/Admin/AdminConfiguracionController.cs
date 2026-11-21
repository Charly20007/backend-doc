using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;
using Surgeon.Domain.Entities;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: datos del doctor, contacto, horarios y SEO global.</summary>
[Route("api/admin/configuracion")]
public class AdminConfiguracionController : AdminControllerBase
{
    private readonly IUnitOfWork _uow;

    public AdminConfiguracionController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet]
    public async Task<ActionResult<ConfiguracionSitioDto>> Get()
    {
        var config = await Obtener();
        var horarios = await _uow.HorariosAtencion.GetAllAsync();
        return Ok((config ?? new ConfiguracionSitio()).AConfiguracionDto(horarios));
    }

    [HttpPut]
    public async Task<ActionResult<ConfiguracionSitioDto>> Actualizar([FromBody] ConfiguracionSitioDto dto)
    {
        var config = await Obtener();
        var nueva = config == null;
        config ??= new ConfiguracionSitio();

        config.AplicarConfiguracion(dto);
        Auditar(config);
        if (nueva) await _uow.ConfiguracionSitio.AddAsync(config);
        else _uow.ConfiguracionSitio.Update(config);
        await _uow.SaveChangesAsync();

        return Ok(config.AConfiguracionDto(await _uow.HorariosAtencion.GetAllAsync()));
    }

    /// <summary>Reemplaza los horarios de atención (se envían todos los tramos).</summary>
    [HttpPut("horarios")]
    public async Task<ActionResult<IEnumerable<HorarioDto>>> ActualizarHorarios([FromBody] List<HorarioDto> horarios)
    {
        foreach (var h in horarios)
        {
            if (TimeSpan.Parse(h.HoraFin) <= TimeSpan.Parse(h.HoraInicio))
                return BadRequest(new { mensaje = $"En el día {h.DiaSemana}, la hora de fin debe ser posterior a la de inicio." });
        }

        foreach (var h in await _uow.HorariosAtencion.GetAllAsync())
        {
            Auditar(h);
            _uow.HorariosAtencion.Remove(h);
        }

        foreach (var h in horarios)
        {
            var horario = new HorarioAtencion
            {
                DiaSemana = h.DiaSemana,
                HoraInicio = TimeSpan.Parse(h.HoraInicio),
                HoraFin = TimeSpan.Parse(h.HoraFin),
                EsActivo = true,
            };
            Auditar(horario);
            await _uow.HorariosAtencion.AddAsync(horario);
        }

        await _uow.SaveChangesAsync();
        var config = await Obtener() ?? new ConfiguracionSitio();
        return Ok(config.AConfiguracionDto(await _uow.HorariosAtencion.GetAllAsync()).Horarios);
    }

    private async Task<ConfiguracionSitio?> Obtener() =>
        (await _uow.ConfiguracionSitio.GetAllAsync()).OrderBy(c => c.Id).FirstOrDefault();
}
