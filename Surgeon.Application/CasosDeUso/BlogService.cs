using Surgeon.Application.Common;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;
using Surgeon.Domain.Entities;

namespace Surgeon.Application.CasosDeUso;

/// <summary>Casos de uso del blog: artículos publicados (web) y gestión de artículos y categorías (CMS).</summary>
public class BlogService
{
    private const string RutaBase = "/blog/";
    private readonly IUnitOfWork _uow;
    private readonly RedireccionesService _redirecciones;

    public BlogService(IUnitOfWork uow, RedireccionesService redirecciones)
    {
        _uow = uow;
        _redirecciones = redirecciones;
    }

    // ---------- Web pública ----------

    /// <summary>Artículos publicados y con fecha ya cumplida, del más reciente al más antiguo.</summary>
    public async Task<IEnumerable<ArticuloBlogDto>> ListarPublicadosAsync(string? categoria = null, string? servicio = null)
    {
        var ahora = DateTime.UtcNow;
        var articulos = await _uow.ArticulosBlog.FindAsync(a => a.PublicadoEnWeb && a.FechaPublicacion <= ahora);
        var (categorias, servicios) = await Mapas();

        if (!string.IsNullOrWhiteSpace(categoria))
            articulos = articulos.Where(a => categorias.TryGetValue(a.IdCategoria, out var c) && c.Slug == categoria);
        if (!string.IsNullOrWhiteSpace(servicio))
            articulos = articulos.Where(a => a.IdServicioMedico is int id && servicios.TryGetValue(id, out var s) && s.Slug == servicio);

        return articulos.OrderByDescending(a => a.FechaPublicacion).Select(a => ADto(a, categorias, servicios)).ToList();
    }

    public async Task<Resultado<ArticuloBlogDto>> ObtenerPublicadoAsync(string slug)
    {
        var ahora = DateTime.UtcNow;
        var articulo = (await _uow.ArticulosBlog.FindAsync(a => a.Slug == slug && a.PublicadoEnWeb && a.FechaPublicacion <= ahora)).FirstOrDefault();
        if (articulo == null)
            return Error.NoEncontrado("Artículo no encontrado");
        return await ADto(articulo);
    }

    public async Task<IEnumerable<CategoriaBlogDto>> ListarCategoriasAsync()
    {
        var categorias = await _uow.CategoriasBlog.GetAllAsync();
        return categorias.OrderBy(c => c.Nombre).Select(ACategoriaDto).ToList();
    }

    // ---------- CMS: artículos (incluye borradores) ----------

    public async Task<IEnumerable<ArticuloBlogDto>> ListarTodosAsync()
    {
        var articulos = await _uow.ArticulosBlog.GetAllAsync();
        var (categorias, servicios) = await Mapas();
        return articulos.OrderByDescending(a => a.FechaPublicacion).Select(a => ADto(a, categorias, servicios)).ToList();
    }

    public async Task<Resultado<ArticuloBlogDto>> ObtenerAsync(int id)
    {
        var articulo = await _uow.ArticulosBlog.GetByIdAsync(id);
        if (articulo == null)
            return Error.NoEncontrado("Artículo no encontrado");
        return await ADto(articulo);
    }

    public async Task<Resultado<ArticuloBlogDto>> CrearAsync(ArticuloBlogGuardarDto dto)
    {
        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Generar(dto.Titulo) : dto.Slug;
        var error = await Validar(dto, slug, idActual: null);
        if (error != null) return error;

        var articulo = new ArticuloBlog();
        Aplicar(articulo, dto, slug);
        await _uow.ArticulosBlog.AddAsync(articulo);
        await _uow.SaveChangesAsync();
        return await ADto(articulo);
    }

    /// <summary>Si cambia el slug, se crea automáticamente una redirección 301 desde la URL anterior.</summary>
    public async Task<Resultado<ArticuloBlogDto>> ActualizarAsync(int id, ArticuloBlogGuardarDto dto)
    {
        var articulo = await _uow.ArticulosBlog.GetByIdAsync(id);
        if (articulo == null)
            return Error.NoEncontrado("Artículo no encontrado");

        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? articulo.Slug : dto.Slug;
        var error = await Validar(dto, slug, idActual: id);
        if (error != null) return error;

        var slugAnterior = articulo.Slug;
        Aplicar(articulo, dto, slug);
        _uow.ArticulosBlog.Update(articulo);

        if (slugAnterior != slug)
            await _redirecciones.RegistrarCambioDeRutaAsync(RutaBase + slugAnterior, RutaBase + slug, $"Cambio de URL del artículo \"{articulo.Titulo}\"");

        await _uow.SaveChangesAsync();
        return await ADto(articulo);
    }

/// <summary>Marca el artículo que se muestra en grande en el blog (desmarca al anterior).</summary>
    public async Task<Resultado<ArticuloBlogDto>> DestacarAsync(int id, bool destacado)
    {
        var articulo = await _uow.ArticulosBlog.GetByIdAsync(id);
        if (articulo == null)
            return Error.NoEncontrado("Artículo no encontrado");

        if (destacado)
        {
            foreach (var otro in await _uow.ArticulosBlog.FindAsync(a => a.Destacado && a.Id != id))
            {
                otro.Destacado = false;
                _uow.ArticulosBlog.Update(otro);
            }
        }

        articulo.Destacado = destacado;
        _uow.ArticulosBlog.Update(articulo);
        await _uow.SaveChangesAsync();
        return await ADto(articulo);
    }

    public async Task<Resultado> EliminarAsync(int id)
    {
        var articulo = await _uow.ArticulosBlog.GetByIdAsync(id);
        if (articulo == null)
            return Error.NoEncontrado("Artículo no encontrado");

        await _redirecciones.DesactivarHaciaAsync(RutaBase + articulo.Slug);
        articulo.Slug = SlugHelper.MarcarEliminado(articulo.Slug, articulo.Id, 200);
        _uow.ArticulosBlog.Remove(articulo);
        await _uow.SaveChangesAsync();
        return Resultado.Ok();
    }

    // ---------- CMS: categorías ----------

    public async Task<Resultado<CategoriaBlogDto>> CrearCategoriaAsync(CategoriaBlogGuardarDto dto)
    {
        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? SlugHelper.Generar(dto.Nombre) : dto.Slug;
        if ((await _uow.CategoriasBlog.FindAsync(c => c.Slug == slug)).Any())
            return Error.Conflicto($"Ya existe una categoría con el slug {slug}.");

        var categoria = new CategoriaBlog { Nombre = dto.Nombre.Trim(), Slug = slug };
        await _uow.CategoriasBlog.AddAsync(categoria);
        await _uow.SaveChangesAsync();
        return ACategoriaDto(categoria);
    }

    public async Task<Resultado<CategoriaBlogDto>> ActualizarCategoriaAsync(int id, CategoriaBlogGuardarDto dto)
    {
        var categoria = await _uow.CategoriasBlog.GetByIdAsync(id);
        if (categoria == null)
            return Error.NoEncontrado("Categoría no encontrada");

        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? categoria.Slug : dto.Slug;
        if ((await _uow.CategoriasBlog.FindAsync(c => c.Slug == slug)).Any(c => c.Id != id))
            return Error.Conflicto($"Ya existe una categoría con el slug {slug}.");

        categoria.Nombre = dto.Nombre.Trim();
        categoria.Slug = slug;
        _uow.CategoriasBlog.Update(categoria);
        await _uow.SaveChangesAsync();
        return ACategoriaDto(categoria);
    }

    public async Task<Resultado> EliminarCategoriaAsync(int id)
    {
        var categoria = await _uow.CategoriasBlog.GetByIdAsync(id);
        if (categoria == null)
            return Error.NoEncontrado("Categoría no encontrada");

        if ((await _uow.ArticulosBlog.FindAsync(a => a.IdCategoria == id)).Any())
            return Error.Conflicto("La categoría tiene artículos. Muévalos a otra categoría antes de eliminarla.");

        categoria.Slug = SlugHelper.MarcarEliminado(categoria.Slug, categoria.Id, 100);
        _uow.CategoriasBlog.Remove(categoria);
        await _uow.SaveChangesAsync();
        return Resultado.Ok();
    }

    // ---------- Reglas y auxiliares ----------

    private async Task<Error?> Validar(ArticuloBlogGuardarDto dto, string slug, int? idActual)
    {
        if (string.IsNullOrEmpty(slug))
            return Error.Validacion("No se pudo generar un slug válido a partir del título.");

        if (await _uow.CategoriasBlog.GetByIdAsync(dto.IdCategoria) == null)
            return Error.Validacion("La categoría seleccionada no existe.");

        if (dto.IdServicioMedico is int idServicio && await _uow.ServiciosMedicos.GetByIdAsync(idServicio) == null)
            return Error.Validacion("El procedimiento relacionado no existe.");

        if ((await _uow.ArticulosBlog.FindAsync(a => a.Slug == slug)).Any(a => a.Id != idActual))
            return Error.Conflicto($"Ya existe otro artículo con la URL /blog/{slug}.");

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

    private async Task<(Dictionary<int, CategoriaBlog> Categorias, Dictionary<int, ServicioMedico> Servicios)> Mapas() =>
        ((await _uow.CategoriasBlog.GetAllAsync()).ToDictionary(c => c.Id), (await _uow.ServiciosMedicos.GetAllAsync()).ToDictionary(s => s.Id));

    private static ArticuloBlogDto ADto(ArticuloBlog a, Dictionary<int, CategoriaBlog> categorias, Dictionary<int, ServicioMedico> servicios) =>
        a.AArticuloDto(categorias.GetValueOrDefault(a.IdCategoria), a.IdServicioMedico is int id ? servicios.GetValueOrDefault(id) : null);

    private async Task<ArticuloBlogDto> ADto(ArticuloBlog a)
    {
        var categoria = await _uow.CategoriasBlog.GetByIdAsync(a.IdCategoria);
        var servicio = a.IdServicioMedico is int id ? await _uow.ServiciosMedicos.GetByIdAsync(id) : null;
        return a.AArticuloDto(categoria, servicio);
    }

    private static CategoriaBlogDto ACategoriaDto(CategoriaBlog c) => new() { Id = c.Id, Nombre = c.Nombre, Slug = c.Slug };
}
