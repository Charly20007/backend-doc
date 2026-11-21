using Surgeon.Application.Common;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Domain.Entities;

namespace Surgeon.Application.CasosDeUso;

/// <summary>Casos de uso de la agenda: solicitud de cita desde la web y gestión de estados en el CMS.</summary>
public class CitasService
{
    private const string EstadoPendiente = "Pendiente";
    private readonly IUnitOfWork _uow;

    public CitasService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    /// <summary>Registra (o reutiliza por correo) al paciente y crea la cita en estado Pendiente.</summary>
    public async Task<Resultado<CitaCreadaDto>> SolicitarAsync(CrearCitaDto dto)
    {
        var servicio = await _uow.ServiciosMedicos.GetByIdAsync(dto.IdServicioMedico);
        if (servicio == null)
            return Error.Validacion("El procedimiento seleccionado no existe.");

        if (dto.FechaHoraInicio < DateTime.Now.AddMinutes(-5))
            return Error.Validacion("La fecha de la cita no puede estar en el pasado.");

        var estadoPendiente = (await _uow.EstadosCita.FindAsync(e => e.Nombre == EstadoPendiente)).FirstOrDefault()
            ?? throw new InvalidOperationException("No existe el estado 'Pendiente'. Cargue Persistence/Datos/datos_iniciales.sql.");

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

        return new CitaCreadaDto { Mensaje = "Cita agendada exitosamente", CitaId = cita.Id };
    }

    public async Task<IEnumerable<CitaRespuestaDto>> ListarAsync()
    {
        var citas = await _uow.Citas.GetAllAsync();
        var pacientes = (await _uow.Pacientes.GetAllAsync()).ToDictionary(p => p.Id);
        var servicios = (await _uow.ServiciosMedicos.GetAllAsync()).ToDictionary(s => s.Id, s => s.Nombre);
        var estados = (await _uow.EstadosCita.GetAllAsync()).ToDictionary(e => e.Id);

        return citas
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
            })
            .ToList();
    }

    public async Task<Resultado<EstadoCitaCambiadoDto>> CambiarEstadoAsync(int id, CambiarEstadoCitaDto dto)
    {
        var cita = await _uow.Citas.GetByIdAsync(id);
        if (cita == null)
            return Error.NoEncontrado("Cita no encontrada");

        var nombre = dto.NuevoEstadoNombre.Trim();
        var estado = (await _uow.EstadosCita.FindAsync(e => e.Nombre == nombre)).FirstOrDefault();
        if (estado == null)
            return Error.Validacion($"Estado no válido: {nombre}");

        cita.IdEstadoCita = estado.Id;
        _uow.Citas.Update(cita);
        await _uow.SaveChangesAsync();
        return new EstadoCitaCambiadoDto { Mensaje = "Estado actualizado", Estado = estado.Nombre, ColorHex = estado.ColorHex };
    }
}
