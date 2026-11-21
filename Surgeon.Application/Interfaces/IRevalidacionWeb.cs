namespace Surgeon.Application.Interfaces;

/// <summary>Avisa a la web pública (Next.js) que el contenido cambió.</summary>
public interface IRevalidacionWeb
{
    Task NotificarCambioAsync(CancellationToken ct = default);
}
