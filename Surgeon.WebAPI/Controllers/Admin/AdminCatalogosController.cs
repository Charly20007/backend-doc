using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: listas para los desplegables de los formularios.</summary>
[Route("api/admin/catalogos")]
public class AdminCatalogosController : AdminControllerBase
{
    private readonly CatalogosService _catalogos;

    public AdminCatalogosController(CatalogosService catalogos) => _catalogos = catalogos;

    [HttpGet]
    public async Task<ActionResult<CatalogosDto>> Get() => Ok(await _catalogos.ObtenerAsync());
}
