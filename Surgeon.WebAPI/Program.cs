using Microsoft.EntityFrameworkCore;
using Surgeon.Application.Interfaces;
using Surgeon.Infrastructure.Persistence;
using Surgeon.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS para permitir conexiones desde los frontends con Tailwind CSS (Vite / Next.js)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApps", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:4173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configuración de Base de Datos SQL Server (por defecto LocalDB o SQL Express local)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=SurgeonDb;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<SurgeonDbContext>(options =>
    options.UseSqlServer(connectionString));

// Inyección de Dependencias para Clean Architecture
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Surgeon Web API - Cirujano General", Version = "v1" });
});

var app = builder.Build();

// Configuración de pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontendApps");
app.UseAuthorization();
app.MapControllers();

app.Run();
