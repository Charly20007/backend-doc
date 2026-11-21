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

    public async Task<IEnumerable<TestimonioDto>> ListarTodosAsync()
    {
        var testimonios = await _uow.Testimonios.GetAllAsync();
        var mapa = await Slugs();
        return testimonios.OrderByDescending(t => t.FechaTestimonio).Select(t => t.ATestimonioDto(mapa)).ToList();
    }

    public async Task<Resultado<TestimonioDto>> CrearAsync(TestimonioGuardarDto dto)
    {
        if (await ServicioInvalido(dto.IdServicioMedico))
            return Error.Validacion("El procedimiento relacionado no existe.");

        var testimonio = new Testimonio();
        Aplicar(testimonio, dto);
        await _uow.Testimonios.AddAsync(testimonio);
        await _uow.SaveChangesAsync();
        return testimonio.ATestimonioDto(await Slugs());
    }

    public async Task<Resultado<TestimonioDto>> ActualizarAsync(int id, TestimonioGuardarDto dto)
    {
        var testimonio = await _uow.Testimonios.GetByIdAsync(id);
        if (testimonio == null)
            return Error.NoEncontrado("Testimonio no encontrado");
        if (await ServicioInvalido(dto.IdServicioMedico))
            return Error.Validacion("El procedimiento relacionado no existe.");

        Aplicar(testimonio, dto);
        _uow.Testimonios.Update(testimonio);
        await _uow.SaveChangesAsync();
        return testimonio.ATestimonioDto(await Slugs());
    }

    /// <summary>Publica u oculta un testimonio en la web sin editarlo.</summary>
    public async Task<Resultado<TestimonioDto>> CambiarAprobacionAsync(int id, bool aprobado)
    {
        var testimonio = await _uow.Testimonios.GetByIdAsync(id);
        if (testimonio == null)
            return Error.NoEncontrado("Testimonio no encontrado");

        testimonio.AprobadoParaWeb = aprobado;
        _uow.Testimonios.Update(testimonio);
        await _uow.SaveChangesAsync();
        return testimonio.ATestimonioDto(await Slugs());
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
    }

    private async Task<bool> ServicioInvalido(int? id) =>
        id is int idServicio && await _uow.ServiciosMedicos.GetByIdAsync(idServicio) == null;

    private async Task<Dictionary<int, string>> Slugs() =>
        (await _uow.ServiciosMedicos.GetAllAsync()).ToDictionary(s => s.Id, s => s.Slug);
}
