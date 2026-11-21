using Surgeon.Application.Common;
using Surgeon.Application.DTOs;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Mapping;
using Surgeon.Domain.Entities;

namespace Surgeon.Application.CasosDeUso;

/// <summary>Casos de uso de los datos del consultorio (una sola fila) y los horarios de atención.</summary>
public class ConfiguracionService
{
    private readonly IUnitOfWork _uow;

    public ConfiguracionService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    /// <summary>Para la web: falla si todavía no se cargaron los datos iniciales.</summary>
    public async Task<Resultado<ConfiguracionSitioDto>> ObtenerAsync()
    {
        var config = await Obtener();
        if (config == null)
            return Error.NoEncontrado("La configuración del sitio no existe. Cargue Persistence/Datos/datos_iniciales.sql.");
        return config.AConfiguracionDto(await _uow.HorariosAtencion.GetAllAsync());
    }

    /// <summary>Para el CMS: si no existe, devuelve un formulario vacío.</summary>
    public async Task<ConfiguracionSitioDto> ObtenerParaEditarAsync()
    {
        var config = await Obtener() ?? new ConfiguracionSitio();
        return config.AConfiguracionDto(await _uow.HorariosAtencion.GetAllAsync());
    }

    public async Task<ConfiguracionSitioDto> ActualizarAsync(ConfiguracionSitioDto dto)
    {
        var config = await Obtener();
        var nueva = config == null;
        config ??= new ConfiguracionSitio();

        config.AplicarConfiguracion(dto);
        if (nueva) await _uow.ConfiguracionSitio.AddAsync(config);
        else _uow.ConfiguracionSitio.Update(config);
        await _uow.SaveChangesAsync();

        return config.AConfiguracionDto(await _uow.HorariosAtencion.GetAllAsync());
    }

    /// <summary>Reemplaza todos los tramos de horario (los anteriores se borran de verdad).</summary>
    public async Task<Resultado<List<HorarioDto>>> ActualizarHorariosAsync(List<HorarioDto> horarios)
    {
        foreach (var h in horarios)
        {
            if (TimeSpan.Parse(h.HoraFin) <= TimeSpan.Parse(h.HoraInicio))
                return Error.Validacion($"En el día {h.DiaSemana}, la hora de fin debe ser posterior a la de inicio.");
        }

        foreach (var h in await _uow.HorariosAtencion.GetAllAsync())
            _uow.HorariosAtencion.RemovePermanently(h);

        foreach (var h in horarios)
        {
            await _uow.HorariosAtencion.AddAsync(new HorarioAtencion
            {
                DiaSemana = h.DiaSemana,
                HoraInicio = TimeSpan.Parse(h.HoraInicio),
                HoraFin = TimeSpan.Parse(h.HoraFin),
                EsActivo = true,
            });
        }

        await _uow.SaveChangesAsync();
        var config = await Obtener() ?? new ConfiguracionSitio();
        return config.AConfiguracionDto(await _uow.HorariosAtencion.GetAllAsync()).Horarios;
    }

    private async Task<ConfiguracionSitio?> Obtener() =>
        (await _uow.ConfiguracionSitio.GetAllAsync()).OrderBy(c => c.Id).FirstOrDefault();
}
