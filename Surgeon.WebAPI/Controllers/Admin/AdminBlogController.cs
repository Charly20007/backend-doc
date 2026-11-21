using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: artículos (incluidos borradores) y categorías del blog.</summary>
[Route("api/admin/blog")]
public class AdminBlogController : AdminControllerBase
{
    private readonly BlogService _blog;

    public AdminBlogController(BlogService blog) => _blog = blog;

    [HttpGet("articulos")]
    public async Task<ActionResult<IEnumerable<ArticuloBlogDto>>> GetArticulos() => Ok(await _blog.ListarTodosAsync());

    [HttpGet("articulos/{id:int}")]
    public async Task<ActionResult<ArticuloBlogDto>> GetArticulo(int id) => Responder(await _blog.ObtenerAsync(id));

    [HttpPost("articulos")]
    public async Task<ActionResult<ArticuloBlogDto>> CrearArticulo([FromBody] ArticuloBlogGuardarDto dto) =>
        Creado(await _blog.CrearAsync(dto), nameof(GetArticulo), a => new { id = a.Id });

    [HttpPut("articulos/{id:int}")]
    public async Task<ActionResult<ArticuloBlogDto>> ActualizarArticulo(int id, [FromBody] ArticuloBlogGuardarDto dto) =>
        Responder(await _blog.ActualizarAsync(id, dto));

    [HttpDelete("articulos/{id:int}")]
    public async Task<ActionResult> EliminarArticulo(int id) => SinContenido(await _blog.EliminarAsync(id));

    [HttpGet("categorias")]
    public async Task<ActionResult<IEnumerable<CategoriaBlogDto>>> GetCategorias() => Ok(await _blog.ListarCategoriasAsync());

    [HttpPost("categorias")]
    public async Task<ActionResult<CategoriaBlogDto>> CrearCategoria([FromBody] CategoriaBlogGuardarDto dto) =>
        Responder(await _blog.CrearCategoriaAsync(dto));

    [HttpPut("categorias/{id:int}")]
    public async Task<ActionResult<CategoriaBlogDto>> ActualizarCategoria(int id, [FromBody] CategoriaBlogGuardarDto dto) =>
        Responder(await _blog.ActualizarCategoriaAsync(id, dto));

    [HttpDelete("categorias/{id:int}")]
    public async Task<ActionResult> EliminarCategoria(int id) => SinContenido(await _blog.EliminarCategoriaAsync(id));
}
