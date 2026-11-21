using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers;

/// <summary>SEO de páginas fijas y redirecciones activas (lectura pública, la usa la web).</summary>
[Route("api/seo")]
public class SeoController : ApiControllerBase
{
    private readonly SeoService _seo;

    public SeoController(SeoService seo) => _seo = seo;

    [HttpGet("paginas")]
    public async Task<ActionResult<IEnumerable<PaginaSeoDto>>> GetPaginas() => Ok(await _seo.ListarPaginasAsync());

    [HttpGet("paginas/{clave}")]
    public async Task<ActionResult<PaginaSeoDto>> GetPagina(string clave) => Responder(await _seo.ObtenerPaginaAsync(clave));

    [HttpGet("redirecciones")]
    public async Task<ActionResult<IEnumerable<RedireccionDto>>> GetRedirecciones() => Ok(await _seo.ListarRedireccionesActivasAsync());
}
