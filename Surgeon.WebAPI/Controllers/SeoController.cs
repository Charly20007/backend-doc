using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;

namespace Surgeon.WebAPI.Controllers;

/// <summary>SEO de páginas fijas y redirecciones activas (lectura pública, la usa la web).</summary>
[ApiController]
[Route("api/seo")]
public class SeoController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public SeoController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet("paginas")]
    public async Task<ActionResult<IEnumerable<PaginaSeoDto>>> GetPaginas()
    {
        var paginas = await _uow.PaginasSeo.GetAllAsync();
        return Ok(paginas.OrderBy(p => p.Id).Select(p => p.APaginaSeoDto()));
    }

    [HttpGet("paginas/{clave}")]
    public async Task<ActionResult<PaginaSeoDto>> GetPagina(string clave)
    {
        var pagina = (await _uow.PaginasSeo.FindAsync(p => p.Clave == clave)).FirstOrDefault();
        return pagina == null ? NotFound(new { mensaje = "Página no encontrada" }) : Ok(pagina.APaginaSeoDto());
    }

    [HttpGet("redirecciones")]
    public async Task<ActionResult<IEnumerable<RedireccionDto>>> GetRedirecciones()
    {
        var redirecciones = await _uow.Redirecciones.FindAsync(r => r.Activa);
        return Ok(redirecciones.Select(r => r.ARedireccionDto()));
    }
}
