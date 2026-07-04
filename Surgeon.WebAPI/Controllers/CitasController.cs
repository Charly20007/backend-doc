using Microsoft.AspNetCore.Mvc;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Domain.Entities;

namespace Surgeon.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CitasController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<ActionResult> CrearCita([FromBody] CrearCitaDto dto)
    {
        // 1. Verificar si el paciente existe o crearlo
        var pacientes = await _unitOfWork.Pacientes.FindAsync(p => p.Correo.ToLower() == dto.Correo.ToLower());
        var paciente = pacientes.FirstOrDefault();

        if (paciente == null)
        {
            paciente = new Paciente
            {
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Correo = dto.Correo,
                Telefono = dto.Telefono
            };
            await _unitOfWork.Pacientes.AddAsync(paciente);
            await _unitOfWork.SaveChangesAsync();
        }

        // 2. Obtener el servicio para saber el tiempo estimado
        var servicio = await _unitOfWork.ServiciosMedicos.GetByIdAsync(dto.IdServicioMedico);
        int duracion = servicio?.TiempoEstimadoMinutos ?? 45;

        // 3. Crear cita con estado Pendiente (Id = 1 por defecto en 02_Carga_Inicial.sql)
        var cita = new Cita
        {
            IdPaciente = paciente.Id,
            IdServicioMedico = dto.IdServicioMedico,
            FechaHoraInicio = dto.FechaHoraInicio,
            FechaHoraFin = dto.FechaHoraInicio.AddMinutes(duracion),
            IdEstadoCita = 1, // Pendiente
            MotivoConsulta = dto.MotivoConsulta,
            PrecioConsulta = servicio?.PrecioReferencial
        };

        await _unitOfWork.Citas.AddAsync(cita);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCitas), new { id = cita.Id }, new { mensaje = "Cita agendada exitosamente", citaId = cita.Id });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CitaRespuestaDto>>> GetCitas()
    {
        var citas = await _unitOfWork.Citas.GetAllAsync();
        var pacientes = (await _unitOfWork.Pacientes.GetAllAsync()).ToDictionary(p => p.Id, p => $"{p.Nombres} {p.Apellidos}");
        var servicios = (await _unitOfWork.ServiciosMedicos.GetAllAsync()).ToDictionary(s => s.Id, s => s.Nombre);

        var respuestas = citas.Select(c => new CitaRespuestaDto
        {
            Id = c.Id,
            PacienteNombreCompleto = pacientes.ContainsKey(c.IdPaciente) ? pacientes[c.IdPaciente] : "Paciente",
            ServicioMedicoNombre = servicios.ContainsKey(c.IdServicioMedico) ? servicios[c.IdServicioMedico] : "Consulta",
            FechaHoraInicio = c.FechaHoraInicio,
            FechaHoraFin = c.FechaHoraFin,
            EstadoCitaNombre = "Pendiente",
            EstadoColorHex = "#F59E0B",
            MotivoConsulta = c.MotivoConsulta
        });

        return Ok(respuestas);
    }
}
