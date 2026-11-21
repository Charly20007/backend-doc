using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;

namespace Surgeon.WebAPI.Controllers;

/// <summary>Blog médico publicado (lectura pública).</summary>
[ApiController]
[Route("api/blog")]
public class BlogController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public BlogController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    /// <summary>Artículos publicados, del más reciente al más antiguo. Filtros opcionales por categoría o procedimiento.</summary>
    [HttpGet("articulos")]
    public async Task<ActionResult<IEnumerable<ArticuloBlogDto>>> GetArticulos([FromQuery] string? categoria = null, [FromQuery] string? servicio = null)
    {
        var ahora = DateTime.UtcNow;
        var articulos = await _uow.ArticulosBlog.FindAsync(a => a.PublicadoEnWeb && a.FechaPublicacion <= ahora);
        var categorias = (await _uow.CategoriasBlog.GetAllAsync()).ToDictionary(c => c.Id);
        var servicios = (await _uow.ServiciosMedicos.GetAllAsync()).ToDictionary(s => s.Id);

        if (!string.IsNullOrWhiteSpace(categoria))
            articulos = articulos.Where(a => categorias.TryGetValue(a.IdCategoria, out var c) && c.Slug == categoria);
        if (!string.IsNullOrWhiteSpace(servicio))
            articulos = articulos.Where(a => a.IdServicioMedico is int id && servicios.TryGetValue(id, out var s) && s.Slug == servicio);

        return Ok(articulos
            .OrderByDescending(a => a.FechaPublicacion)
            .Select(a => a.AArticuloDto(categorias.GetValueOrDefault(a.IdCategoria), a.IdServicioMedico is int id ? servicios.GetValueOrDefault(id) : null)));
    }

    [HttpGet("articulos/{slug}")]
    public async Task<ActionResult<ArticuloBlogDto>> GetArticulo(string slug)
    {
        var ahora = DateTime.UtcNow;
        var articulo = (await _uow.ArticulosBlog.FindAsync(a => a.Slug == slug && a.PublicadoEnWeb && a.FechaPublicacion <= ahora)).FirstOrDefault();
        if (articulo == null)
            return NotFound(new { mensaje = "Artículo no encontrado" });

        var categoria = await _uow.CategoriasBlog.GetByIdAsync(articulo.IdCategoria);
        var servicio = articulo.IdServicioMedico is int id ? await _uow.ServiciosMedicos.GetByIdAsync(id) : null;
        return Ok(articulo.AArticuloDto(categoria, servicio));
    }

    [HttpGet("categorias")]
    public async Task<ActionResult<IEnumerable<CategoriaBlogDto>>> GetCategorias()
    {
        var categorias = await _uow.CategoriasBlog.GetAllAsync();
        return Ok(categorias.OrderBy(c => c.Nombre).Select(c => new CategoriaBlogDto { Id = c.Id, Nombre = c.Nombre, Slug = c.Slug }));
    }
}
