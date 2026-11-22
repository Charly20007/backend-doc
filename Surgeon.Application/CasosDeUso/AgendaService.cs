using Surgeon.Application.Common;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Domain.Entities;

namespace Surgeon.Application.CasosDeUso;

/// <summary>
/// Agenda de turnos: calcula los turnos libres a partir de
/// horarios de atención − citas que ocupan turno − bloqueos − reglas de anticipación.
/// </summary>
public class AgendaService
{
    private readonly IUnitOfWork _uow;
    private readonly TimeProvider _reloj;

    public AgendaService(IUnitOfWork uow, TimeProvider reloj)
    {
        _uow = uow;
        _reloj = reloj;
    }

    /// <summary>Días y horas libres desde hoy hasta el máximo configurado.</summary>
    public async Task<DisponibilidadDto> ObtenerDisponibilidadAsync()
    {
        var reglas = await Reglas();
        var dto = new DisponibilidadDto { ReservasActivas = reglas.Activas, DuracionTurnoMinutos = reglas.Duracion };
        if (!reglas.Activas)
            return dto;

        var ahora = HoraLocal.Ahora(_reloj);
        var desde = ahora.Date;
        var hasta = desde.AddDays(reglas.DiasMaximos);
        var datos = await Ocupacion(desde, hasta.AddDays(1));

        for (var dia = desde; dia <= hasta; dia = dia.AddDays(1))
        {
            var libres = TurnosDelDia(dia, reglas, datos, ahora).Select(t => t.ToString("HH:mm")).ToList();
            if (libres.Count > 0)
                dto.Dias.Add(new DiaDisponibleDto { Fecha = dia.ToString("yyyy-MM-dd"), Turnos = libres });
        }
        return dto;
    }

    /// <summary>Comprueba que la hora pedida sea un turno libre válido. Devuelve el fin del turno.</summary>
    public async Task<Resultado<DateTime>> ValidarTurnoAsync(DateTime inicio)
    {
        var reglas = await Reglas();
        if (!reglas.Activas)
            return Error.Validacion("Las reservas en línea no están disponibles. Por favor, llámenos para agendar.");

        var ahora = HoraLocal.Ahora(_reloj);
        if (inicio.Date < ahora.Date || inicio.Date > ahora.Date.AddDays(reglas.DiasMaximos))
            return Error.Validacion("La fecha elegida está fuera del periodo de reservas.");

        var datos = await Ocupacion(inicio.Date, inicio.Date.AddDays(1));
        var turnos = TurnosDelDia(inicio.Date, reglas, datos, ahora);
        if (turnos.Contains(inicio))
            return inicio.AddMinutes(reglas.Duracion);

        // ¿Era un turno válido pero ya está ocupado? Mensaje distinto para que el paciente elija otro
        var todos = TurnosDelDia(inicio.Date, reglas, (Array.Empty<Cita>(), datos.Bloqueos, datos.Horarios), ahora);
        return todos.Contains(inicio)
            ? Error.Conflicto("Ese horario acaba de ocuparse. Por favor, elija otro turno.")
            : Error.Validacion("La hora elegida no es un turno disponible.");
    }

    // ---------- Bloqueos (vacaciones, congresos, quirófano) ----------

    public async Task<IEnumerable<BloqueoDto>> ListarBloqueosAsync()
    {
        var desde = HoraLocal.Ahora(_reloj).Date;
        var bloqueos = await _uow.BloqueosHorario.FindAsync(b => b.FechaFin >= desde);
        return bloqueos.OrderBy(b => b.FechaInicio).Select(ABloqueoDto).ToList();
    }

    public async Task<Resultado<BloqueoDto>> CrearBloqueoAsync(BloqueoGuardarDto dto)
    {
        if (dto.FechaFin <= dto.FechaInicio)
            return Error.Validacion("El fin del bloqueo debe ser posterior al inicio.");

        var bloqueo = new BloqueoHorario { FechaInicio = dto.FechaInicio, FechaFin = dto.FechaFin, Motivo = dto.Motivo.Trim() };
        await _uow.BloqueosHorario.AddAsync(bloqueo);
        await _uow.SaveChangesAsync();
        return ABloqueoDto(bloqueo);
    }

    public async Task<Resultado> EliminarBloqueoAsync(int id)
    {
        var bloqueo = await _uow.BloqueosHorario.GetByIdAsync(id);
        if (bloqueo == null)
            return Error.NoEncontrado("Bloqueo no encontrado");

        _uow.BloqueosHorario.RemovePermanently(bloqueo);
        await _uow.SaveChangesAsync();
        return Resultado.Ok();
    }

    // ---------- Cálculo ----------

    private sealed record ReglasAgenda(bool Activas, int Duracion, int AnticipacionHoras, int DiasMaximos);

    private async Task<ReglasAgenda> Reglas()
    {
        var c = (await _uow.ConfiguracionSitio.GetAllAsync()).OrderBy(x => x.Id).FirstOrDefault() ?? new ConfiguracionSitio();
        return new ReglasAgenda(c.ReservasOnlineActivas, Math.Max(10, c.DuracionTurnoMinutos), Math.Max(0, c.AnticipacionMinimaHoras), Math.Max(1, c.DiasMaximosReserva));
    }

    private async Task<(IEnumerable<Cita> Citas, IEnumerable<BloqueoHorario> Bloqueos, IEnumerable<HorarioAtencion> Horarios)> Ocupacion(DateTime desde, DateTime hasta) => (
        await _uow.Citas.FindAsync(c => c.OcupaTurno && c.FechaHoraInicio < hasta && c.FechaHoraFin > desde),
        await _uow.BloqueosHorario.FindAsync(b => b.FechaInicio < hasta && b.FechaFin > desde),
        await _uow.HorariosAtencion.FindAsync(h => h.EsActivo));

    /// <summary>Turnos libres de un día: cada tramo de horario se divide en bloques de la duración del turno.</summary>
    private static List<DateTime> TurnosDelDia(DateTime dia, ReglasAgenda reglas,
        (IEnumerable<Cita> Citas, IEnumerable<BloqueoHorario> Bloqueos, IEnumerable<HorarioAtencion> Horarios) datos, DateTime ahora)
    {
        var diaSemana = dia.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)dia.DayOfWeek; // 1 = lunes … 7 = domingo
        var minimo = ahora.AddHours(reglas.AnticipacionHoras);
        var duracion = TimeSpan.FromMinutes(reglas.Duracion);
        var libres = new List<DateTime>();

        foreach (var tramo in datos.Horarios.Where(h => h.DiaSemana == diaSemana).OrderBy(h => h.HoraInicio))
        {
            for (var inicio = dia + tramo.HoraInicio; inicio + duracion <= dia + tramo.HoraFin; inicio += duracion)
            {
                var fin = inicio + duracion;
                if (inicio < minimo) continue;
                if (datos.Bloqueos.Any(b => b.FechaInicio < fin && b.FechaFin > inicio)) continue;
                if (datos.Citas.Any(c => c.FechaHoraInicio < fin && c.FechaHoraFin > inicio)) continue;
                libres.Add(inicio);
            }
        }
        return libres;
    }

    private static BloqueoDto ABloqueoDto(BloqueoHorario b) => new() { Id = b.Id, FechaInicio = b.FechaInicio, FechaFin = b.FechaFin, Motivo = b.Motivo };
}
