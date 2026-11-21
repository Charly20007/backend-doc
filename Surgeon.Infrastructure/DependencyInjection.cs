using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Settings;
using Surgeon.Infrastructure.Persistence;
using Surgeon.Infrastructure.Repositories;
using Surgeon.Infrastructure.Security;

namespace Surgeon.Infrastructure;

/// <summary>
/// Registra las implementaciones técnicas: base de datos (PostgreSQL), repositorios,
/// seguridad (BCrypt, JWT) y el aviso de revalidación a la web.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Fechas como "timestamp without time zone" (igual que el antiguo SQL Server): acepta DateTime de cualquier tipo
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Falta ConnectionStrings:DefaultConnection (ver appsettings.Development.local.example.json).");

        services.AddScoped<AuditoriaInterceptor>();
        services.AddDbContext<SurgeonDbContext>((sp, options) => options
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(sp.GetRequiredService<AuditoriaInterceptor>()));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<AdminSeeder>();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Seccion));
        services.Configure<AdminInicialSettings>(configuration.GetSection(AdminInicialSettings.Seccion));
        services.Configure<WebSettings>(configuration.GetSection(WebSettings.Seccion));

        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddHttpClient<IRevalidacionWeb, RevalidacionWeb>(c => c.Timeout = TimeSpan.FromSeconds(10));

        return services;
    }
}
