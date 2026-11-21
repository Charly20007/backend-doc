using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;

namespace Surgeon.WebAPI.Controllers;

/// <summary>Datos globales del sitio: doctor, contacto, horarios y SEO por defecto (lectura pública).</summary>
[ApiController]
[Route("api/[controller]")]
public class ConfiguracionController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public ConfiguracionController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet]
    public async Task<ActionResult<ConfiguracionSitioDto>> Get()
    {
        var config = (await _uow.ConfiguracionSitio.GetAllAsync()).OrderBy(c => c.Id).FirstOrDefault();
        if (config == null)
            return NotFound(new { mensaje = "La configuración del sitio no existe. Ejecute 03_Fase1_SEO.sql." });

        var horarios = await _uow.HorariosAtencion.GetAllAsync();
        return Ok(config.AConfiguracionDto(horarios));
    }
}
