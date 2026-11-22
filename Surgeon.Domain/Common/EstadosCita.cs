namespace Surgeon.Domain.Common;

/// <summary>Nombres de los estados de cita (tabla estados_cita) y qué estados ocupan un turno de la agenda.</summary>
public static class EstadosCita
{
    public const string Pendiente = "Pendiente";
    public const string Confirmada = "Confirmada";
    public const string Atendida = "Atendida";
    public const string Cancelada = "Cancelada";

    /// <summary>Una cita cancelada libera su turno; cualquier otro estado lo mantiene ocupado.</summary>
    public static bool OcupaTurno(string estado) => !string.Equals(estado, Cancelada, StringComparison.OrdinalIgnoreCase);
}
