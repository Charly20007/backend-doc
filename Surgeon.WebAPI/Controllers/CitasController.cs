using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Domain.Common;
using Surgeon.Domain.Entities;

namespace Surgeon.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private const string EstadoPendiente = "Pendiente";
    private readonly IUnitOfWork _uow;

    public CitasController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    /// <summary>Solicitud de cita desde la web pública.</summary>
    [HttpPost]
    [AllowAnonymous]
    [EnableRateLimiting("formularios")]
    public async Task<ActionResult> CrearCita([FromBody] CrearCitaDto dto)
    {
        var servicio = await _uow.ServiciosMedicos.GetByIdAsync(dto.IdServicioMedico);
        if (servicio == null)
            return BadRequest(new { mensaje = "El procedimiento seleccionado no existe." });

        if (dto.FechaHoraInicio < DateTime.Now.AddMinutes(-5))
            return BadRequest(new { mensaje = "La fecha de la cita no puede estar en el pasado." });

        var estadoPendiente = (await _uow.EstadosCita.FindAsync(e => e.Nombre == EstadoPendiente)).FirstOrDefault();
        if (estadoPendiente == null)
            return Problem("No existe el estado 'Pendiente'. Ejecute 02_Carga_Inicial.sql.");

        // 1. Buscar al paciente por correo o registrarlo
        var correo = dto.Correo.Trim().ToLowerInvariant();
        var paciente = (await _uow.Pacientes.FindAsync(p => p.Correo == correo)).FirstOrDefault();
        if (paciente == null)
        {
            paciente = new Paciente
            {
                Nombres = dto.Nombres.Trim(),
                Apellidos = dto.Apellidos.Trim(),
                Correo = correo,
                Telefono = dto.Telefono.Trim(),
            };
            await _uow.Pacientes.AddAsync(paciente);
            await _uow.SaveChangesAsync();
        }

        // 2. Crear la cita en estado Pendiente
        var cita = new Cita
        {
            IdPaciente = paciente.Id,
            IdServicioMedico = servicio.Id,
            FechaHoraInicio = dto.FechaHoraInicio,
            FechaHoraFin = dto.FechaHoraInicio.AddMinutes(servicio.TiempoEstimadoMinutos),
            IdEstadoCita = estadoPendiente.Id,
            MotivoConsulta = dto.MotivoConsulta?.Trim(),
            PrecioConsulta = servicio.PrecioReferencial,
        };

        await _uow.Citas.AddAsync(cita);
        await _uow.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created, new { mensaje = "Cita agendada exitosamente", citaId = cita.Id });
    }

    /// <summary>Agenda de citas (CMS). Contiene datos de pacientes: requiere sesión.</summary>
    [HttpGet]
    [Authorize(Roles = RolesSistema.Agenda)]
    public async Task<ActionResult<IEnumerable<CitaRespuestaDto>>> GetCitas()
    {
        var citas = await _uow.Citas.GetAllAsync();
        var pacientes = (await _uow.Pacientes.GetAllAsync()).ToDictionary(p => p.Id);
        var servicios = (await _uow.ServiciosMedicos.GetAllAsync()).ToDictionary(s => s.Id, s => s.Nombre);
        var estados = (await _uow.EstadosCita.GetAllAsync()).ToDictionary(e => e.Id);

        var respuestas = citas
            .OrderByDescending(c => c.FechaHoraInicio)
            .Select(c =>
            {
                pacientes.TryGetValue(c.IdPaciente, out var paciente);
                estados.TryGetValue(c.IdEstadoCita, out var estado);
                return new CitaRespuestaDto
                {
                    Id = c.Id,
                    PacienteNombreCompleto = paciente != null ? $"{paciente.Nombres} {paciente.Apellidos}" : "Paciente",
                    PacienteTelefono = paciente?.Telefono,
                    PacienteCorreo = paciente?.Correo,
                    ServicioMedicoNombre = servicios.GetValueOrDefault(c.IdServicioMedico, "Consulta"),
                    FechaHoraInicio = c.FechaHoraInicio,
                    FechaHoraFin = c.FechaHoraFin,
                    EstadoCitaNombre = estado?.Nombre ?? EstadoPendiente,
                    EstadoColorHex = estado?.ColorHex ?? "#F59E0B",
                    MotivoConsulta = c.MotivoConsulta,
                };
            });

        return Ok(respuestas);
    }

    /// <summary>Cambia el estado de una cita (Pendiente, Confirmada, Atendida, Cancelada).</summary>
    [HttpPut("{id:int}/estado")]
    [Authorize(Roles = RolesSistema.Agenda)]
    public async Task<ActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoCitaDto dto)
    {
        var cita = await _uow.Citas.GetByIdAsync(id);
        if (cita == null)
            return NotFound(new { mensaje = "Cita no encontrada" });

        var nombre = dto.NuevoEstadoNombre.Trim();
        var estado = (await _uow.EstadosCita.FindAsync(e => e.Nombre == nombre)).FirstOrDefault();
        if (estado == null)
            return BadRequest(new { mensaje = $"Estado no válido: {nombre}" });

        cita.IdEstadoCita = estado.Id;
        _uow.Citas.Update(cita);
        await _uow.SaveChangesAsync();
        return Ok(new { mensaje = "Estado actualizado", estado = estado.Nombre, colorHex = estado.ColorHex });
    }
}
