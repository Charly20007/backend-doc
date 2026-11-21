using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;

namespace Surgeon.Application.CasosDeUso;

/// <summary>Listas para los desplegables del CMS.</summary>
public class CatalogosService
{
    private readonly IUnitOfWork _uow;

    public CatalogosService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<CatalogosDto> ObtenerAsync() => new()
    {
        Especialidades = (await _uow.Especialidades.GetAllAsync()).OrderBy(e => e.Nombre)
            .Select(e => new ItemCatalogoDto { Id = e.Id, Nombre = e.Nombre, Extra = e.Icono }).ToList(),
        CategoriasBlog = (await _uow.CategoriasBlog.GetAllAsync()).OrderBy(c => c.Nombre)
            .Select(c => new ItemCatalogoDto { Id = c.Id, Nombre = c.Nombre, Extra = c.Slug }).ToList(),
        EstadosCita = (await _uow.EstadosCita.GetAllAsync()).OrderBy(e => e.Id)
            .Select(e => new ItemCatalogoDto { Id = e.Id, Nombre = e.Nombre, Extra = e.ColorHex }).ToList(),
        Servicios = (await _uow.ServiciosMedicos.GetAllAsync()).OrderBy(s => s.Orden).ThenBy(s => s.Id)
            .Select(s => new ItemCatalogoDto { Id = s.Id, Nombre = s.Nombre, Extra = s.Slug }).ToList(),
    };
}
