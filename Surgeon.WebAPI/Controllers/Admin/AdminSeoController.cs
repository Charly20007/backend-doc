using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;
using Surgeon.Domain.Entities;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: SEO de páginas fijas y redirecciones.</summary>
[Route("api/admin/seo")]
public class AdminSeoController : AdminControllerBase
{
    private readonly IUnitOfWork _uow;

    public AdminSeoController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    // ---------- Páginas fijas ----------

    [HttpGet("paginas")]
    public async Task<ActionResult<IEnumerable<PaginaSeoDto>>> GetPaginas()
    {
        var paginas = await _uow.PaginasSeo.GetAllAsync();
        return Ok(paginas.OrderBy(p => p.Id).Select(p => p.APaginaSeoDto()));
    }

    /// <summary>Las páginas fijas las define la web; desde el CMS solo se edita su SEO.</summary>
    [HttpPut("paginas/{clave}")]
    public async Task<ActionResult<PaginaSeoDto>> ActualizarPagina(string clave, [FromBody] SeoDto dto)
    {
        var pagina = (await _uow.PaginasSeo.FindAsync(p => p.Clave == clave)).FirstOrDefault();
        if (pagina == null)
            return NoEncontrado("Página no encontrada");

        pagina.AplicarSeo(dto);
        Auditar(pagina);
        _uow.PaginasSeo.Update(pagina);
        await _uow.SaveChangesAsync();
        return Ok(pagina.APaginaSeoDto());
    }

    // ---------- Redirecciones ----------

    [HttpGet("redirecciones")]
    public async Task<ActionResult<IEnumerable<RedireccionDto>>> GetRedirecciones()
    {
        var redirecciones = await _uow.Redirecciones.GetAllAsync();
        return Ok(redirecciones.OrderByDescending(r => r.FechaModificacion).Select(r => r.ARedireccionDto()));
    }

    [HttpPost("redirecciones")]
    public async Task<ActionResult<RedireccionDto>> CrearRedireccion([FromBody] RedireccionGuardarDto dto)
    {
        var error = await Validar(dto, idActual: null);
        if (error != null) return error;

        var redireccion = new Redireccion();
        Aplicar(redireccion, dto);
        Auditar(redireccion);
        await _uow.Redirecciones.AddAsync(redireccion);
        await _uow.SaveChangesAsync();
        return Ok(redireccion.ARedireccionDto());
    }

    [HttpPut("redirecciones/{id:int}")]
    public async Task<ActionResult<RedireccionDto>> ActualizarRedireccion(int id, [FromBody] RedireccionGuardarDto dto)
    {
        var redireccion = await _uow.Redirecciones.GetByIdAsync(id);
        if (redireccion == null)
            return NoEncontrado("Redirección no encontrada");

        var error = await Validar(dto, idActual: id);
        if (error != null) return error;

        Aplicar(redireccion, dto);
        Auditar(redireccion);
        _uow.Redirecciones.Update(redireccion);
        await _uow.SaveChangesAsync();
        return Ok(redireccion.ARedireccionDto());
    }

    [HttpDelete("redirecciones/{id:int}")]
    public async Task<ActionResult> EliminarRedireccion(int id)
    {
        var redireccion = await _uow.Redirecciones.GetByIdAsync(id);
        if (redireccion == null)
            return NoEncontrado("Redirección no encontrada");

        _uow.Redirecciones.RemovePermanently(redireccion);
        await _uow.SaveChangesAsync();
        return NoContent();
    }

    private async Task<ActionResult?> Validar(RedireccionGuardarDto dto, int? idActual)
    {
        var origen = Normalizar(dto.RutaOrigen);
        var destino = dto.RutaDestino.StartsWith('/') ? Normalizar(dto.RutaDestino) : dto.RutaDestino.Trim();

        if (origen == "/")
            return BadRequest(new { mensaje = "No se puede redirigir la portada." });
        if (origen == destino)
            return BadRequest(new { mensaje = "El origen y el destino no pueden ser iguales." });
        if ((await _uow.Redirecciones.FindAsync(r => r.RutaOrigen == origen)).Any(r => r.Id != idActual))
            return Conflicto($"Ya existe una redirección desde {origen}.");
        if ((await _uow.Redirecciones.FindAsync(r => r.RutaOrigen == destino && r.Activa)).Any(r => r.Id != idActual))
            return BadRequest(new { mensaje = $"El destino {destino} a su vez redirige a otra página: apunte directamente a la página final." });

        return null;
    }

    private static void Aplicar(Redireccion r, RedireccionGuardarDto dto)
    {
        r.RutaOrigen = Normalizar(dto.RutaOrigen);
        r.RutaDestino = dto.RutaDestino.StartsWith('/') ? Normalizar(dto.RutaDestino) : dto.RutaDestino.Trim();
        r.EsPermanente = dto.EsPermanente;
        r.Activa = dto.Activa;
        r.Nota = Mapeos.Limpiar(dto.Nota);
    }

    /// <summary>"/Blog/Mi-Articulo/" → "/blog/mi-articulo"</summary>
    private static string Normalizar(string ruta)
    {
        var r = ruta.Trim().ToLowerInvariant();
        return r.Length > 1 ? r.TrimEnd('/') : r;
    }
}
