using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: edición de las páginas legales (privacidad, términos, consentimiento).</summary>
[Route("api/admin/paginas-legales")]
public class AdminPaginasLegalesController : AdminControllerBase
{
    private readonly PaginasLegalesService _paginas;

    public AdminPaginasLegalesController(PaginasLegalesService paginas) => _paginas = paginas;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaginaLegalResumenDto>>> GetTodas() => Ok(await _paginas.ListarTodasAsync());

    [HttpGet("{clave}")]
    public async Task<ActionResult<PaginaLegalDto>> Get(string clave) => Responder(await _paginas.ObtenerAsync(clave));

    [HttpPut("{clave}")]
    public async Task<ActionResult<PaginaLegalDto>> Actualizar(string clave, [FromBody] PaginaLegalGuardarDto dto) =>
        Responder(await _paginas.ActualizarAsync(clave, dto));
}
