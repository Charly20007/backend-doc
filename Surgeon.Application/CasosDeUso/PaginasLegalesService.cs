using Surgeon.Application.Common;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;

namespace Surgeon.Application.CasosDeUso;

/// <summary>Casos de uso de las páginas legales (privacidad, términos, consentimiento).</summary>
public class PaginasLegalesService
{
    private readonly IUnitOfWork _uow;

    public PaginasLegalesService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<PaginaLegalResumenDto>> ListarPublicadasAsync()
    {
        var paginas = await _uow.PaginasLegales.FindAsync(p => p.Publicada);
        return paginas.OrderBy(p => p.Id).Select(p => p.APaginaLegalResumenDto()).ToList();
    }

    public async Task<Resultado<PaginaLegalDto>> ObtenerPublicadaAsync(string clave)
    {
        var pagina = (await _uow.PaginasLegales.FindAsync(p => p.Clave == clave && p.Publicada)).FirstOrDefault();
        if (pagina == null)
            return Error.NoEncontrado("Página no encontrada");
        return pagina.APaginaLegalDto();
    }

    public async Task<IEnumerable<PaginaLegalResumenDto>> ListarTodasAsync()
    {
        var paginas = await _uow.PaginasLegales.GetAllAsync();
        return paginas.OrderBy(p => p.Id).Select(p => p.APaginaLegalResumenDto()).ToList();
    }

    public async Task<Resultado<PaginaLegalDto>> ObtenerAsync(string clave)
    {
        var pagina = (await _uow.PaginasLegales.FindAsync(p => p.Clave == clave)).FirstOrDefault();
        if (pagina == null)
            return Error.NoEncontrado("Página no encontrada");
        return pagina.APaginaLegalDto();
    }

    /// <summary>Las rutas son fijas (las define la web); aquí solo se edita el contenido.</summary>
    public async Task<Resultado<PaginaLegalDto>> ActualizarAsync(string clave, PaginaLegalGuardarDto dto)
    {
        var pagina = (await _uow.PaginasLegales.FindAsync(p => p.Clave == clave)).FirstOrDefault();
        if (pagina == null)
            return Error.NoEncontrado("Página no encontrada");

        pagina.Titulo = dto.Titulo.Trim();
        pagina.ContenidoHtml = dto.ContenidoHtml;
        pagina.Publicada = dto.Publicada;
        pagina.AplicarSeo(dto.Seo);
        _uow.PaginasLegales.Update(pagina);
        await _uow.SaveChangesAsync();
        return pagina.APaginaLegalDto();
    }
}
