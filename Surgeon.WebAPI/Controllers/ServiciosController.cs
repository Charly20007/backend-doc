using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Domain.Entities;

namespace Surgeon.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiciosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ServiciosController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServicioMedicoDto>>> GetServicios([FromQuery] bool soloDestacados = false)
    {
        var servicios = await _unitOfWork.ServiciosMedicos.GetAllAsync();
        
        if (soloDestacados)
        {
            servicios = servicios.Where(s => s.DestacadoEnWeb);
        }

        var especialidades = await _unitOfWork.Especialidades.GetAllAsync();
        var especialidadesMap = especialidades.ToDictionary(e => e.Id, e => e.Nombre);

        var dtos = servicios.Select(s => new ServicioMedicoDto
        {
            Id = s.Id,
            Nombre = s.Nombre,
            Slug = s.Slug,
            Resumen = s.Resumen,
            DescripcionDetallada = s.DescripcionDetallada,
            TiempoEstimadoMinutos = s.TiempoEstimadoMinutos,
            PrecioReferencial = s.PrecioReferencial,
            Icono = s.Icono,
            EspecialidadNombre = especialidadesMap.ContainsKey(s.IdEspecialidad) ? especialidadesMap[s.IdEspecialidad] : "General",
            DestacadoEnWeb = s.DestacadoEnWeb
        });

        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServicioMedicoDto>> GetServicioById(int id)
    {
        var servicio = await _unitOfWork.ServiciosMedicos.GetByIdAsync(id);
        if (servicio == null)
            return NotFound(new { mensaje = "Servicio médico no encontrado" });

        var especialidad = await _unitOfWork.Especialidades.GetByIdAsync(servicio.IdEspecialidad);

        return Ok(new ServicioMedicoDto
        {
            Id = servicio.Id,
            Nombre = servicio.Nombre,
            Slug = servicio.Slug,
            Resumen = servicio.Resumen,
            DescripcionDetallada = servicio.DescripcionDetallada,
            TiempoEstimadoMinutos = servicio.TiempoEstimadoMinutos,
            PrecioReferencial = servicio.PrecioReferencial,
            Icono = servicio.Icono,
            EspecialidadNombre = especialidad?.Nombre ?? "General",
            DestacadoEnWeb = servicio.DestacadoEnWeb
        });
    }
}
