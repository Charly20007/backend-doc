namespace Surgeon.Application.Common;

/// <summary>
/// Hora del consultorio (Lima, UTC-5). Las citas se guardan en hora local de Lima,
/// así que "ahora" no puede depender de la zona horaria del servidor.
/// </summary>
public static class HoraLocal
{
    private static readonly TimeZoneInfo Zona = ObtenerZona();

    public static DateTime Ahora(TimeProvider? reloj = null) =>
        TimeZoneInfo.ConvertTimeFromUtc((reloj ?? TimeProvider.System).GetUtcNow().UtcDateTime, Zona);

    private static TimeZoneInfo ObtenerZona()
    {
        foreach (var id in new[] { "America/Lima", "SA Pacific Standard Time" })
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById(id); }
            catch (TimeZoneNotFoundException) { }
        }
        return TimeZoneInfo.CreateCustomTimeZone("Lima", TimeSpan.FromHours(-5), "Lima", "Lima");
    }
}
