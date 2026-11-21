using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.CasosDeUso;
using Surgeon.Application.DTOs;
using Surgeon.WebAPI.Infrastructure;

namespace Surgeon.WebAPI.Controllers.Admin;

/// <summary>CMS: alta, edición (contenido + SEO) y baja de procedimientos.</summary>
[Route("api/admin/servicios")]
public class AdminServiciosController : AdminControllerBase
{
    private readonly ProcedimientosService _procedimientos;

    public AdminServiciosController(ProcedimientosService procedimientos) => _procedimientos = procedimientos;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServicioMedicoDto>>> GetTodos() => Ok(await _procedimientos.ListarAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServicioDetalleDto>> GetDetalle(int id) => Responder(await _procedimientos.ObtenerDetalleAsync(id));

    [HttpPost]
    public async Task<ActionResult<ServicioDetalleDto>> Crear([FromBody] ServicioGuardarDto dto) =>
        Creado(await _procedimientos.CrearAsync(dto), nameof(GetDetalle), s => new { id = s.Id });

    /// <summary>Si cambia el slug, se crea automáticamente una redirección 301 desde la URL anterior.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ServicioDetalleDto>> Actualizar(int id, [FromBody] ServicioGuardarDto dto) =>
        Responder(await _procedimientos.ActualizarAsync(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Eliminar(int id) => SinContenido(await _procedimientos.EliminarAsync(id));
}
