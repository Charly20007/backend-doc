using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Surgeon.Application;
using Surgeon.Application.Interfaces;
using Surgeon.Application.Settings;
using Surgeon.Infrastructure;
using Surgeon.Infrastructure.Persistence;
using Surgeon.WebAPI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Secretos locales (no versionados): appsettings.{Entorno}.local.json
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.local.json", optional: true, reloadOnChange: true);

// ---------- CORS: orígenes permitidos de la web pública y el CMS ----------
var origenes = builder.Configuration.GetSection("Cors:Origenes").Get<string[]>()
    ?? new[] { "http://localhost:3000", "http://localhost:5173", "http://localhost:4173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApps", policy =>
    {
        policy.WithOrigins(origenes)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ---------- Capas de Clean Architecture ----------
// Application: casos de uso · Infrastructure: PostgreSQL, repositorios, seguridad · WebAPI: HTTP
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Usuario de la petición actual (lo usan los casos de uso y la auditoría automática)
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioActual, UsuarioActualHttp>();

// ---------- Autenticación JWT para el CMS ----------
var jwt = builder.Configuration.GetSection(JwtSettings.Seccion).Get<JwtSettings>() ?? new JwtSettings();
if (string.IsNullOrWhiteSpace(jwt.Key) || jwt.Key.Length < 32)
    throw new InvalidOperationException(
        "Falta la clave JWT (Jwt:Key, mínimo 32 caracteres). En producción defínala con la variable de entorno Jwt__Key.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            NameClaimType = "name",
            RoleClaimType = "role",
        };
    });
builder.Services.AddAuthorization();

// ---------- Límite de peticiones (fuerza bruta en login y spam en formularios) ----------
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        await context.HttpContext.Response.WriteAsJsonAsync(
            new { mensaje = "Demasiados intentos. Espere un momento e inténtelo de nuevo." }, token);
    };

    options.AddPolicy("login", http => RateLimitPartition.GetFixedWindowLimiter(
        http.Connection.RemoteIpAddress?.ToString() ?? "desconocido",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(1) }));

    options.AddPolicy("formularios", http => RateLimitPartition.GetFixedWindowLimiter(
        http.Connection.RemoteIpAddress?.ToString() ?? "desconocido",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 10, Window = TimeSpan.FromMinutes(10) }));
});

builder.Services.AddControllers(o => o.Filters.Add<RevalidarWebFilter>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Surgeon Web API - Cirujano General", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Pegue el token obtenido en POST /api/auth/login",
    });
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>(),
    });
});

var app = builder.Build();

// Aplica las migraciones pendientes y crea el administrador inicial si no hay usuarios
// (si la BD no está disponible, la API arranca igual y lo informa en el log)
using (var scope = app.Services.CreateScope())
{
    try
    {
        await scope.ServiceProvider.GetRequiredService<SurgeonDbContext>().Database.MigrateAsync();
        await scope.ServiceProvider.GetRequiredService<AdminSeeder>().EjecutarAsync();
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "No se pudo preparar la base de datos (¿PostgreSQL está encendido?).");
    }
}

// ---------- Pipeline HTTP ----------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontendApps");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
