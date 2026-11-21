using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers;

/// <summary>Datos globales del sitio: doctor, contacto, horarios y SEO por defecto (lectura pública).</summary>
[Route("api/[controller]")]
public class ConfiguracionController : ApiControllerBase
{
    private readonly ConfiguracionService _configuracion;

    public ConfiguracionController(ConfiguracionService configuracion) => _configuracion = configuracion;

    [HttpGet]
    public async Task<ActionResult<ConfiguracionSitioDto>> Get() => Responder(await _configuracion.ObtenerAsync());
}
