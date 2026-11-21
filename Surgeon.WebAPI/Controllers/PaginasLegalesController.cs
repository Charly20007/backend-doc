using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers;

/// <summary>Páginas legales publicadas (lectura pública, las usa la web).</summary>
[Route("api/paginas-legales")]
public class PaginasLegalesController : ApiControllerBase
{
    private readonly PaginasLegalesService _paginas;

    public PaginasLegalesController(PaginasLegalesService paginas) => _paginas = paginas;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaginaLegalResumenDto>>> GetPublicadas() => Ok(await _paginas.ListarPublicadasAsync());

    [HttpGet("{clave}")]
    public async Task<ActionResult<PaginaLegalDto>> Get(string clave) => Responder(await _paginas.ObtenerPublicadaAsync(clave));
}
