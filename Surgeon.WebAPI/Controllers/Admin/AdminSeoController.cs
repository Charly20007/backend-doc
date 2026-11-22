using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: SEO de páginas fijas y redirecciones.</summary>
[Route("api/admin/seo")]
public class AdminSeoController : AdminControllerBase
{
    private readonly SeoService _seo;

    public AdminSeoController(SeoService seo) => _seo = seo;

    [HttpGet("paginas")]
    public async Task<ActionResult<IEnumerable<PaginaSeoDto>>> GetPaginas() => Ok(await _seo.ListarPaginasAsync());

    /// <summary>Las páginas fijas las define la web; desde el CMS solo se edita su SEO.</summary>
    [HttpPut("paginas/{clave}")]
    public async Task<ActionResult<PaginaSeoDto>> ActualizarPagina(string clave, [FromBody] SeoDto dto) =>
        Responder(await _seo.ActualizarPaginaAsync(clave, dto));

/// <summary>Textos visibles de la página (título, descripción…).</summary>
    [HttpPut("paginas/{clave}/textos")]
    public async Task<ActionResult<PaginaSeoDto>> ActualizarTextos(string clave, [FromBody] PaginaTextosDto dto) =>
        Responder(await _seo.ActualizarTextosAsync(clave, dto));

    [HttpGet("redirecciones")]
    public async Task<ActionResult<IEnumerable<RedireccionDto>>> GetRedirecciones() => Ok(await _seo.ListarRedireccionesAsync());

    [HttpPost("redirecciones")]
    public async Task<ActionResult<RedireccionDto>> CrearRedireccion([FromBody] RedireccionGuardarDto dto) =>
        Responder(await _seo.CrearRedireccionAsync(dto));

    [HttpPut("redirecciones/{id:int}")]
    public async Task<ActionResult<RedireccionDto>> ActualizarRedireccion(int id, [FromBody] RedireccionGuardarDto dto) =>
        Responder(await _seo.ActualizarRedireccionAsync(id, dto));

    [HttpDelete("redirecciones/{id:int}")]
    public async Task<ActionResult> EliminarRedireccion(int id) => SinContenido(await _seo.EliminarRedireccionAsync(id));
}
