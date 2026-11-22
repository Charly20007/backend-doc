using Surgeon.Application.Common;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;
using Surgeon.Domain.Entities;

namespace Surgeon.Application.CasosDeUso;

/// <summary>Casos de uso de testimonios: los aprobados para la web y la moderación en el CMS.</summary>
public class TestimoniosService
{
    private readonly IUnitOfWork _uow;

    public TestimoniosService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    /// <param name="servicio">Slug del procedimiento para filtrar (opcional).</param>
    public async Task<IEnumerable<TestimonioDto>> ListarAprobadosAsync(string? servicio = null)
    {
        var testimonios = await _uow.Testimonios.FindAsync(t => t.AprobadoParaWeb);
        var mapa = await Slugs();
        var dtos = testimonios.OrderByDescending(t => t.FechaTestimonio).Select(t => t.ATestimonioDto(mapa)).ToList();
        return string.IsNullOrWhiteSpace(servicio) ? dtos : dtos.Where(t => t.ProcedimientoSlug == servicio).ToList();
    }

    /// <summary>Todos los testimonios para el CMS: primero los pendientes de revisión.</summary>
    public async Task<IEnumerable<TestimonioAdminDto>> ListarTodosAsync()
    {
        var testimonios = await _uow.Testimonios.GetAllAsync();
        var mapa = await Slugs();
        return testimonios
            .OrderByDescending(t => t.PendienteRevision)
            .ThenByDescending(t => t.FechaTestimonio)
            .Select(t => t.ATestimonioAdminDto(mapa))
            .ToList();
    }

    /// <summary>Cuántos testimonios enviados desde la web esperan revisión (aviso del menú del CMS).</summary>
    public async Task<PendientesDto> ContarPendientesAsync() =>
        new() { Cantidad = (await _uow.Testimonios.FindAsync(t => t.PendienteRevision)).Count() };

    /// <summary>
    /// Un paciente envía su testimonio desde la web. Nunca se publica directamente:
    /// queda oculto y pendiente hasta que el administrador lo apruebe en el CMS.
    /// </summary>
    public async Task<Resultado> EnviarDesdeWebAsync(TestimonioEnviarDto dto)
    {
        // Campo trampa lleno → bot. Se responde como si todo fuera bien, pero no se guarda.
        if (!string.IsNullOrWhiteSpace(dto.Sitio))
            return Resultado.Ok();

        if (!dto.AutorizaPublicacion)
            return Error.Validacion("Para publicar tu testimonio necesitamos tu autorización.");

        string procedimiento;
        if (dto.IdServicioMedico is int idServicio)
        {
            var servicio = await _uow.ServiciosMedicos.GetByIdAsync(idServicio);
            if (servicio == null)
                return Error.Validacion("El procedimiento elegido no existe.");
            procedimiento = servicio.Nombre;
        }
        else
        {
            procedimiento = string.IsNullOrWhiteSpace(dto.Procedimiento) ? "Paciente" : dto.Procedimiento.Trim();
        }

        await _uow.Testimonios.AddAsync(new Testimonio
        {
            NombrePaciente = dto.NombrePaciente.Trim(),
            Procedimiento = procedimiento,
            IdServicioMedico = dto.IdServicioMedico,
            CalificacionEstrellas = dto.CalificacionEstrellas,
            Comentario = dto.Comentario.Trim(),
            FechaTestimonio = DateTime.UtcNow,
            AprobadoParaWeb = false,
            EnviadoDesdeWeb = true,
            PendienteRevision = true,
            AutorizaPublicacion = true,
            ContactoPaciente = string.IsNullOrWhiteSpace(dto.ContactoPaciente) ? null : dto.ContactoPaciente.Trim(),
        });
        await _uow.SaveChangesAsync();
        return Resultado.Ok();
    }

    public async Task<Resultado<TestimonioAdminDto>> CrearAsync(TestimonioGuardarDto dto)
    {
        if (await ServicioInvalido(dto.IdServicioMedico))
            return Error.Validacion("El procedimiento relacionado no existe.");

        var testimonio = new Testimonio();
        Aplicar(testimonio, dto);
        await _uow.Testimonios.AddAsync(testimonio);
        await _uow.SaveChangesAsync();
        return testimonio.ATestimonioAdminDto(await Slugs());
    }

    public async Task<Resultado<TestimonioAdminDto>> ActualizarAsync(int id, TestimonioGuardarDto dto)
    {
        var testimonio = await _uow.Testimonios.GetByIdAsync(id);
        if (testimonio == null)
            return Error.NoEncontrado("Testimonio no encontrado");
        if (await ServicioInvalido(dto.IdServicioMedico))
            return Error.Validacion("El procedimiento relacionado no existe.");

        Aplicar(testimonio, dto);
        _uow.Testimonios.Update(testimonio);
        await _uow.SaveChangesAsync();
        return testimonio.ATestimonioAdminDto(await Slugs());
    }

    /// <summary>Publica u oculta un testimonio en la web sin editarlo. En ambos casos queda revisado.</summary>
    public async Task<Resultado<TestimonioAdminDto>> CambiarAprobacionAsync(int id, bool aprobado)
    {
        var testimonio = await _uow.Testimonios.GetByIdAsync(id);
        if (testimonio == null)
            return Error.NoEncontrado("Testimonio no encontrado");

        testimonio.AprobadoParaWeb = aprobado;
        testimonio.PendienteRevision = false;
        _uow.Testimonios.Update(testimonio);
        await _uow.SaveChangesAsync();
        return testimonio.ATestimonioAdminDto(await Slugs());
    }

    public async Task<Resultado> EliminarAsync(int id)
    {
        var testimonio = await _uow.Testimonios.GetByIdAsync(id);
        if (testimonio == null)
            return Error.NoEncontrado("Testimonio no encontrado");

        _uow.Testimonios.Remove(testimonio);
        await _uow.SaveChangesAsync();
        return Resultado.Ok();
    }

    private static void Aplicar(Testimonio t, TestimonioGuardarDto dto)
    {
        t.NombrePaciente = dto.NombrePaciente.Trim();
        t.Procedimiento = dto.Procedimiento.Trim();
        t.IdServicioMedico = dto.IdServicioMedico;
        t.CalificacionEstrellas = dto.CalificacionEstrellas;
        t.Comentario = dto.Comentario.Trim();
        t.FechaTestimonio = dto.FechaTestimonio ?? (t.Id == 0 ? DateTime.UtcNow : t.FechaTestimonio);
        t.AprobadoParaWeb = dto.AprobadoParaWeb;
        // Publicarlo al editar cuenta como revisarlo
        if (dto.AprobadoParaWeb)
            t.PendienteRevision = false;
    }

    private async Task<bool> ServicioInvalido(int? id) =>
        id is int idServicio && await _uow.ServiciosMedicos.GetByIdAsync(idServicio) == null;

    private async Task<Dictionary<int, string>> Slugs() =>
        (await _uow.ServiciosMedicos.GetAllAsync()).ToDictionary(s => s.Id, s => s.Slug);
}
