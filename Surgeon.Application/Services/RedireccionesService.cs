using Surgeon.Application.Interfaces;
using Surgeon.Domain.Entities;

namespace Surgeon.Application.Services;

/// <summary>
/// Mantiene las redirecciones 301 cuando cambia la URL de una página,
/// para no perder el posicionamiento ganado en Google.
/// </summary>
public class RedireccionesService
{
    private readonly IUnitOfWork _uow;

    public RedireccionesService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    /// <summary>
    /// Registra (sin guardar) la redirección de rutaAnterior a rutaNueva:
    ///  - las redirecciones que apuntaban a la ruta anterior pasan a apuntar a la nueva (evita cadenas);
    ///  - se desactiva cualquier redirección cuyo origen sea la nueva ruta (la página ahora existe);
    ///  - si ya existía una redirección desde la ruta anterior, se actualiza.
    /// </summary>
    public async Task RegistrarCambioDeRutaAsync(string rutaAnterior, string rutaNueva, string nota)
    {
        if (string.Equals(rutaAnterior, rutaNueva, StringComparison.OrdinalIgnoreCase))
            return;

        foreach (var r in await _uow.Redirecciones.FindAsync(r => r.RutaDestino == rutaAnterior))
        {
            r.RutaDestino = rutaNueva;
            _uow.Redirecciones.Update(r);
        }

        foreach (var r in await _uow.Redirecciones.FindAsync(r => r.RutaOrigen == rutaNueva))
        {
            r.Activa = false;
            _uow.Redirecciones.Update(r);
        }

        var existente = (await _uow.Redirecciones.FindAsync(r => r.RutaOrigen == rutaAnterior)).FirstOrDefault();
        if (existente != null)
        {
            existente.RutaDestino = rutaNueva;
            existente.Activa = true;
            existente.EsPermanente = true;
            existente.Nota = nota;
            _uow.Redirecciones.Update(existente);
        }
        else
        {
            await _uow.Redirecciones.AddAsync(new Redireccion
            {
                RutaOrigen = rutaAnterior,
                RutaDestino = rutaNueva,
                EsPermanente = true,
                Activa = true,
                Nota = nota,
            });
        }
    }

    /// <summary>
    /// Desactiva (sin guardar) las redirecciones que llevan a una página eliminada,
    /// para que no envíen a los visitantes a un 404.
    /// </summary>
    public async Task DesactivarHaciaAsync(string ruta)
    {
        foreach (var r in await _uow.Redirecciones.FindAsync(r => r.RutaDestino == ruta && r.Activa))
        {
            r.Activa = false;
            r.Nota = "Desactivada: la página de destino fue eliminada";
            _uow.Redirecciones.Update(r);
        }
    }
}
