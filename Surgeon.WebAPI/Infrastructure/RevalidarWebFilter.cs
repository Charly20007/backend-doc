using Microsoft.AspNetCore.Mvc.Filters;
using Surgeon.Application.Interfaces;

namespace Surgeon.WebAPI.Infrastructure;

/// <summary>
/// Tras cada cambio exitoso en /api/admin/* (POST, PUT, DELETE), avisa a la web
/// para que muestre el contenido nuevo sin esperar ni recompilar.
/// </summary>
public class RevalidarWebFilter : IAsyncResultFilter
{
    private readonly IServiceScopeFactory _scopes;

    public RevalidarWebFilter(IServiceScopeFactory scopes)
    {
        _scopes = scopes;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        await next();

        var http = context.HttpContext;
        var esCambio = !HttpMethods.IsGet(http.Request.Method) && !HttpMethods.IsHead(http.Request.Method);
        var esAdmin = http.Request.Path.StartsWithSegments("/api/admin");
        if (!esCambio || !esAdmin || http.Response.StatusCode >= 300)
            return;

        // En segundo plano: la respuesta al CMS no espera a la web
        _ = Task.Run(async () =>
        {
            using var scope = _scopes.CreateScope();
            await scope.ServiceProvider.GetRequiredService<IRevalidacionWeb>().NotificarCambioAsync();
        });
    }
}
