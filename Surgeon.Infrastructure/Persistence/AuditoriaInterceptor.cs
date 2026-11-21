using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Surgeon.Application.Interfaces;
using Surgeon.Domain.Common;

namespace Surgeon.Infrastructure.Persistence;

/// <summary>
/// Completa las columnas de auditoría (usuario, IP y fecha de modificación) de toda entidad
/// que se inserte o modifique. Así ningún caso de uso tiene que acordarse de hacerlo.
/// </summary>
public class AuditoriaInterceptor : SaveChangesInterceptor
{
    private readonly IUsuarioActual _usuario;

    public AuditoriaInterceptor(IUsuarioActual usuario)
    {
        _usuario = usuario;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        Auditar(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Auditar(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    private void Auditar(DbContext? contexto)
    {
        if (contexto is null) return;

        // Sin sesión (formulario público de citas) se registra como "web"
        var usuario = _usuario.Correo?.Split('@')[0] ?? "web";
        usuario = usuario.Length > 20 ? usuario[..20] : usuario;
        var ip = _usuario.Ip.Length > 15 ? _usuario.Ip[..15] : _usuario.Ip;
        var ahora = DateTime.UtcNow;

        foreach (var entrada in contexto.ChangeTracker.Entries<BaseEntity>())
        {
            if (entrada.State is not (EntityState.Added or EntityState.Modified)) continue;
            entrada.Entity.UsuarioModificacion = usuario;
            entrada.Entity.IPModificacion = ip;
            entrada.Entity.FechaModificacion = ahora;
        }
    }
}
