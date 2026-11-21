using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.Common;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;
using Surgeon.Application.Services;
using Surgeon.Domain.Entities;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: artículos (incluidos borradores) y categorías del blog.</summary>
[Route("api/admin/blog")]
public class AdminBlogController : AdminControllerBase
{
    private const string RutaBase = "/blog/";
    private readonly IUnitOfWork _uow;
    private readonly RedireccionesService _redirecciones;

    public AdminBlogController(IUnitOfWork uow, RedireccionesService redirecciones)
    {
        _uow = uow;
        _redirecciones = redirecciones;
    }

    // ---------- Artículos ----------

    [HttpGet("articulos")]
    public async Task<ActionResult<IEnumerable<ArticuloBlogDto>>> GetArticulos()
    {
        var articulos = await _uow.ArticulosBlog.GetAllAsync();
        var categorias = (await _uow.CategoriasBlog.GetAllAsync()).ToDictionary(c => c.Id);
        var servicios = (await _uow.ServiciosMedicos.GetAllAsync()).ToDictionary(s => s.Id);

        return Ok(articulos
            .OrderByDescending(a => a.FechaPublicacion)
            .Select(a => a.AArticuloDto(categorias.GetValueOrDefault(a.IdCategoria), a.IdServicioMedico is int id ? servicios.GetValueOrDefault(id) : null)));
    }

    [HttpGet("articulos/{id:int}")]
    public async Task<ActionResult<ArticuloBlogDto>> GetArticulo(int id)
    {
        var articulo = await _uow.ArticulosBlog.GetByIdAsync(id);
        return articulo == null ? NoEncontrado("Artículo no encontrado") : Ok(await ADto(articulo));
    }

    [HttpPost("articulos")]
    public async Task<ActionResult<ArticuloBlogDto>> CrearArticulo([FromBody] ArticuloBlogGuardarDto dto)
    {
        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Generar(dto.Titulo) : dto.Slug;
        var error = await Validar(dto, slug, idActual: null);
        if (error != null) return error;

        var articulo = new ArticuloBlog();
        Aplicar(articulo, dto, slug);
        Auditar(articulo);
        await _uow.ArticulosBlog.AddAsync(articulo);
        await _uow.SaveChangesAsync();

        return CreatedAtAction(nameof(GetArticulo), new { id = articulo.Id }, await ADto(articulo));
    }

    [HttpPut("articulos/{id:int}")]
    public async Task<ActionResult<ArticuloBlogDto>> ActualizarArticulo(int id, [FromBody] ArticuloBlogGuardarDto dto)
    {
        var articulo = await _uow.ArticulosBlog.GetByIdAsync(id);
        if (articulo == null)
            return NoEncontrado("Artículo no encontrado");

        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? articulo.Slug : dto.Slug;
        var error = await Validar(dto, slug, idActual: id);
        if (error != null) return error;

        var slugAnterior = articulo.Slug;
        Aplicar(articulo, dto, slug);
        Auditar(articulo);
        _uow.ArticulosBlog.Update(articulo);

        if (slugAnterior != slug)
            await _redirecciones.RegistrarCambioDeRutaAsync(RutaBase + slugAnterior, RutaBase + slug, $"Cambio de URL del artículo \"{articulo.Titulo}\"");

        await _uow.SaveChangesAsync();
        return Ok(await ADto(articulo));
    }

    [HttpDelete("articulos/{id:int}")]
    public async Task<ActionResult> EliminarArticulo(int id)
    {
        var articulo = await _uow.ArticulosBlog.GetByIdAsync(id);
        if (articulo == null)
            return NoEncontrado("Artículo no encontrado");

        await _redirecciones.DesactivarHaciaAsync(RutaBase + articulo.Slug);

        var sufijo = $"--eliminado-{articulo.Id}";
        articulo.Slug = (articulo.Slug.Length + sufijo.Length > 200 ? articulo.Slug[..(200 - sufijo.Length)] : articulo.Slug) + sufijo;
        Auditar(articulo);
        _uow.ArticulosBlog.Remove(articulo);
        await _uow.SaveChangesAsync();
        return NoContent();
    }

    // ---------- Categorías ----------

    [HttpGet("categorias")]
    public async Task<ActionResult<IEnumerable<CategoriaBlogDto>>> GetCategorias()
    {
        var categorias = await _uow.CategoriasBlog.GetAllAsync();
        return Ok(categorias.OrderBy(c => c.Nombre).Select(c => new CategoriaBlogDto { Id = c.Id, Nombre = c.Nombre, Slug = c.Slug }));
    }

    [HttpPost("categorias")]
    public async Task<ActionResult<CategoriaBlogDto>> CrearCategoria([FromBody] CategoriaBlogGuardarDto dto)
    {
        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Generar(dto.Nombre) : dto.Slug;
        if ((await _uow.CategoriasBlog.FindAsync(c => c.Slug == slug)).Any())
            return Conflicto($"Ya existe una categoría con el slug {slug}.");

        var categoria = new CategoriaBlog { Nombre = dto.Nombre.Trim(), Slug = slug };
        Auditar(categoria);
        await _uow.CategoriasBlog.AddAsync(categoria);
        await _uow.SaveChangesAsync();
        return Ok(new CategoriaBlogDto { Id = categoria.Id, Nombre = categoria.Nombre, Slug = categoria.Slug });
    }

    [HttpPut("categorias/{id:int}")]
    public async Task<ActionResult<CategoriaBlogDto>> ActualizarCategoria(int id, [FromBody] CategoriaBlogGuardarDto dto)
    {
        var categoria = await _uow.CategoriasBlog.GetByIdAsync(id);
        if (categoria == null)
            return NoEncontrado("Categoría no encontrada");

        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? categoria.Slug : dto.Slug;
        if ((await _uow.CategoriasBlog.FindAsync(c => c.Slug == slug)).Any(c => c.Id != id))
            return Conflicto($"Ya existe una categoría con el slug {slug}.");

        categoria.Nombre = dto.Nombre.Trim();
        categoria.Slug = slug;
        Auditar(categoria);
        _uow.CategoriasBlog.Update(categoria);
        await _uow.SaveChangesAsync();
        return Ok(new CategoriaBlogDto { Id = categoria.Id, Nombre = categoria.Nombre, Slug = categoria.Slug });
    }

    [HttpDelete("categorias/{id:int}")]
    public async Task<ActionResult> EliminarCategoria(int id)
    {
        var categoria = await _uow.CategoriasBlog.GetByIdAsync(id);
        if (categoria == null)
            return NoEncontrado("Categoría no encontrada");

        if ((await _uow.ArticulosBlog.FindAsync(a => a.IdCategoria == id)).Any())
            return Conflicto("La categoría tiene artículos. Muévalos a otra categoría antes de eliminarla.");

        var sufijo = $"--eliminado-{categoria.Id}";
        categoria.Slug = (categoria.Slug.Length + sufijo.Length > 100 ? categoria.Slug[..(100 - sufijo.Length)] : categoria.Slug) + sufijo;
        Auditar(categoria);
        _uow.CategoriasBlog.Remove(categoria);
        await _uow.SaveChangesAsync();
        return NoContent();
    }

    // ---------- Auxiliares ----------

    private async Task<ActionResult?> Validar(ArticuloBlogGuardarDto dto, string slug, int? idActual)
    {
        if (string.IsNullOrEmpty(slug))
            return BadRequest(new { mensaje = "No se pudo generar un slug válido a partir del título." });

        if (await _uow.CategoriasBlog.GetByIdAsync(dto.IdCategoria) == null)
            return BadRequest(new { mensaje = "La categoría seleccionada no existe." });

        if (dto.IdServicioMedico is int idServicio && await _uow.ServiciosMedicos.GetByIdAsync(idServicio) == null)
            return BadRequest(new { mensaje = "El procedimiento relacionado no existe." });

        if ((await _uow.ArticulosBlog.FindAsync(a => a.Slug == slug)).Any(a => a.Id != idActual))
            return Conflicto($"Ya existe otro artículo con la URL /blog/{slug}.");

        return null;
    }

    private static void Aplicar(ArticuloBlog a, ArticuloBlogGuardarDto dto, string slug)
    {
        a.Titulo = dto.Titulo.Trim();
        a.Slug = slug;
        a.Resumen = dto.Resumen.Trim();
        a.ContenidoHtml = dto.ContenidoHtml;
        a.ImagenPortadaUrl = Mapeos.Limpiar(dto.ImagenPortadaUrl);
        a.IdCategoria = dto.IdCategoria;
        a.IdServicioMedico = dto.IdServicioMedico;
        a.Autor = Mapeos.Limpiar(dto.Autor) ?? a.Autor;
        a.TiempoLecturaMinutos = dto.TiempoLecturaMinutos ?? Mapeos.CalcularTiempoLectura(dto.ContenidoHtml);
        a.FechaPublicacion = dto.FechaPublicacion ?? (a.Id == 0 ? DateTime.UtcNow : a.FechaPublicacion);
        a.PublicadoEnWeb = dto.PublicadoEnWeb;
        a.AplicarSeo(dto.Seo);
    }

    private async Task<ArticuloBlogDto> ADto(ArticuloBlog a)
    {
        var categoria = await _uow.CategoriasBlog.GetByIdAsync(a.IdCategoria);
        var servicio = a.IdServicioMedico is int id ? await _uow.ServiciosMedicos.GetByIdAsync(id) : null;
        return a.AArticuloDto(categoria, servicio);
    }
}
