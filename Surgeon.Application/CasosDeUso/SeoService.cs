using Surgeon.Application.Common;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;
using Surgeon.Domain.Entities;

namespace Surgeon.Application.CasosDeUso;

/// <summary>Casos de uso de SEO: páginas fijas y redirecciones.</summary>
public class SeoService
{
    private readonly IUnitOfWork _uow;

    public SeoService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    // ---------- Páginas fijas ----------

    public async Task<IEnumerable<PaginaSeoDto>> ListarPaginasAsync()
    {
        var paginas = await _uow.PaginasSeo.GetAllAsync();
        return paginas.OrderBy(p => p.Id).Select(p => p.APaginaSeoDto()).ToList();
    }

    public async Task<Resultado<PaginaSeoDto>> ObtenerPaginaAsync(string clave)
    {
        var pagina = (await _uow.PaginasSeo.FindAsync(p => p.Clave == clave)).FirstOrDefault();
        if (pagina == null)
            return Error.NoEncontrado("Página no encontrada");
        return pagina.APaginaSeoDto();
    }

    /// <summary>Las páginas fijas las define la web; desde el CMS solo se edita su SEO.</summary>
    public async Task<Resultado<PaginaSeoDto>> ActualizarPaginaAsync(string clave, SeoDto dto)
    {
        var pagina = (await _uow.PaginasSeo.FindAsync(p => p.Clave == clave)).FirstOrDefault();
        if (pagina == null)
            return Error.NoEncontrado("Página no encontrada");

        pagina.AplicarSeo(dto);
        _uow.PaginasSeo.Update(pagina);
        await _uow.SaveChangesAsync();
        return pagina.APaginaSeoDto();
    }

    // ---------- Redirecciones ----------

    public async Task<IEnumerable<RedireccionDto>> ListarRedireccionesActivasAsync()
    {
        var redirecciones = await _uow.Redirecciones.FindAsync(r => r.Activa);
        return redirecciones.Select(r => r.ARedireccionDto()).ToList();
    }

    public async Task<IEnumerable<RedireccionDto>> ListarRedireccionesAsync()
    {
        var redirecciones = await _uow.Redirecciones.GetAllAsync();
        return redirecciones.OrderByDescending(r => r.FechaModificacion).Select(r => r.ARedireccionDto()).ToList();
    }

    public async Task<Resultado<RedireccionDto>> CrearRedireccionAsync(RedireccionGuardarDto dto)
    {
        var error = await Validar(dto, idActual: null);
        if (error != null) return error;

        var redireccion = new Redireccion();
        Aplicar(redireccion, dto);
        await _uow.Redirecciones.AddAsync(redireccion);
        await _uow.SaveChangesAsync();
        return redireccion.ARedireccionDto();
    }

    public async Task<Resultado<RedireccionDto>> ActualizarRedireccionAsync(int id, RedireccionGuardarDto dto)
    {
        var redireccion = await _uow.Redirecciones.GetByIdAsync(id);
        if (redireccion == null)
            return Error.NoEncontrado("Redirección no encontrada");

        var error = await Validar(dto, idActual: id);
        if (error != null) return error;

        Aplicar(redireccion, dto);
        _uow.Redirecciones.Update(redireccion);
        await _uow.SaveChangesAsync();
        return redireccion.ARedireccionDto();
    }

    public async Task<Resultado> EliminarRedireccionAsync(int id)
    {
        var redireccion = await _uow.Redirecciones.GetByIdAsync(id);
        if (redireccion == null)
            return Error.NoEncontrado("Redirección no encontrada");

        _uow.Redirecciones.RemovePermanently(redireccion);
        await _uow.SaveChangesAsync();
        return Resultado.Ok();
    }

    private async Task<Error?> Validar(RedireccionGuardarDto dto, int? idActual)
    {
        var origen = Normalizar(dto.RutaOrigen);
        var destino = Destino(dto.RutaDestino);

        if (origen == "/")
            return Error.Validacion("No se puede redirigir la portada.");
        if (origen == destino)
            return Error.Validacion("El origen y el destino no pueden ser iguales.");
        if ((await _uow.Redirecciones.FindAsync(r => r.RutaOrigen == origen)).Any(r => r.Id != idActual))
            return Error.Conflicto($"Ya existe una redirección desde {origen}.");
        if ((await _uow.Redirecciones.FindAsync(r => r.RutaOrigen == destino && r.Activa)).Any(r => r.Id != idActual))
            return Error.Validacion($"El destino {destino} a su vez redirige a otra página: apunte directamente a la página final.");

        return null;
    }

    private static void Aplicar(Redireccion r, RedireccionGuardarDto dto)
    {
        r.RutaOrigen = Normalizar(dto.RutaOrigen);
        r.RutaDestino = Destino(dto.RutaDestino);
        r.EsPermanente = dto.EsPermanente;
        r.Activa = dto.Activa;
        r.Nota = Mapeos.Limpiar(dto.Nota);
    }

    private static string Destino(string ruta) => ruta.StartsWith('/') ? Normalizar(ruta) : ruta.Trim();

    /// <summary>"/Blog/Mi-Articulo/" → "/blog/mi-articulo"</summary>
    private static string Normalizar(string ruta)
    {
        var r = ruta.Trim().ToLowerInvariant();
        return r.Length > 1 ? r.TrimEnd('/') : r;
    }
}
