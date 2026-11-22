using Microsoft.Extensions.DependencyInjection;
using Surgeon.Application.CasosDeUso;

namespace Surgeon.Application;

/// <summary>Registra los casos de uso de la capa Application.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RedireccionesService>();
        services.AddScoped<ProcedimientosService>();
        services.AddScoped<BlogService>();
        services.AddScoped<TestimoniosService>();
        services.AddScoped<ConfiguracionService>();
        services.AddScoped<SeoService>();
        services.AddScoped<PaginasLegalesService>();
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<AgendaService>();
        services.AddScoped<CitasService>();
        services.AddScoped<AuthService>();
        services.AddScoped<CatalogosService>();
        return services;
    }
}
