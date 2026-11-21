using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers;

/// <summary>Blog médico publicado (lectura pública).</summary>
[Route("api/blog")]
public class BlogController : ApiControllerBase
{
    private readonly BlogService _blog;

    public BlogController(BlogService blog) => _blog = blog;

    /// <summary>Artículos publicados. Filtros opcionales por categoría o procedimiento (slug).</summary>
    [HttpGet("articulos")]
    public async Task<ActionResult<IEnumerable<ArticuloBlogDto>>> GetArticulos([FromQuery] string? categoria = null, [FromQuery] string? servicio = null) =>
        Ok(await _blog.ListarPublicadosAsync(categoria, servicio));

    [HttpGet("articulos/{slug}")]
    public async Task<ActionResult<ArticuloBlogDto>> GetArticulo(string slug) =>
        Responder(await _blog.ObtenerPublicadoAsync(slug));

    [HttpGet("categorias")]
    public async Task<ActionResult<IEnumerable<CategoriaBlogDto>>> GetCategorias() =>
        Ok(await _blog.ListarCategoriasAsync());
}
