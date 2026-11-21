using Microsoft.EntityFrameworkCore;
using Surgeon.Domain.Entities;

namespace Surgeon.Infrastructure.Persistence;

public class SurgeonDbContext : DbContext
{
    public SurgeonDbContext(DbContextOptions<SurgeonDbContext> options) : base(options)
    {
    }

    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Especialidad> Especialidades => Set<Especialidad>();
    public DbSet<ServicioMedico> ServiciosMedicos => Set<ServicioMedico>();
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<EstadoCita> EstadosCita => Set<EstadoCita>();
    public DbSet<Cita> Citas => Set<Cita>();
    public DbSet<HorarioAtencion> HorariosAtencion => Set<HorarioAtencion>();
    public DbSet<BloqueoHorario> BloqueosHorario => Set<BloqueoHorario>();
    public DbSet<CategoriaBlog> CategoriasBlog => Set<CategoriaBlog>();
    public DbSet<ArticuloBlog> ArticulosBlog => Set<ArticuloBlog>();
    public DbSet<Testimonio> Testimonios => Set<Testimonio>();
    public DbSet<MensajeContacto> MensajesContacto => Set<MensajeContacto>();
    public DbSet<ServicioContenido> ServiciosContenido => Set<ServicioContenido>();
    public DbSet<ConfiguracionSitio> ConfiguracionSitio => Set<ConfiguracionSitio>();
    public DbSet<PaginaSeo> PaginasSeo => Set<PaginaSeo>();
    public DbSet<Redireccion> Redirecciones => Set<Redireccion>();
    public DbSet<PaginaLegal> PaginasLegales => Set<PaginaLegal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tablas y columnas en snake_case (convención de PostgreSQL), configurado en Program.cs

        modelBuilder.Entity<PaginaLegal>().HasIndex(p => p.Clave).IsUnique();

        modelBuilder.Entity<HorarioAtencion>().Property(h => h.HoraInicio).HasColumnType("time");
        modelBuilder.Entity<HorarioAtencion>().Property(h => h.HoraFin).HasColumnType("time");

        modelBuilder.Entity<CategoriaBlog>().HasIndex(c => c.Slug).IsUnique();
        modelBuilder.Entity<Usuario>().HasIndex(u => u.Correo).IsUnique();
        modelBuilder.Entity<Rol>().HasIndex(r => r.Nombre).IsUnique();

        modelBuilder.Entity<ServicioMedico>().Property(s => s.PrecioReferencial).HasPrecision(10, 2);
        modelBuilder.Entity<Cita>().Property(c => c.PrecioConsulta).HasPrecision(10, 2);

        // Configuración de índices y relaciones
        modelBuilder.Entity<ServicioMedico>()
            .HasIndex(s => s.Slug)
            .IsUnique();

        modelBuilder.Entity<ArticuloBlog>()
            .HasIndex(a => a.Slug)
            .IsUnique();

        modelBuilder.Entity<ServicioMedico>()
            .HasOne(s => s.Especialidad)
            .WithMany(e => e.ServiciosMedicos)
            .HasForeignKey(s => s.IdEspecialidad)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Paciente)
            .WithMany(p => p.Citas)
            .HasForeignKey(c => c.IdPaciente)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cita>()
            .HasOne(c => c.ServicioMedico)
            .WithMany(s => s.Citas)
            .HasForeignKey(c => c.IdServicioMedico)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cita>()
            .HasOne(c => c.EstadoCita)
            .WithMany(e => e.Citas)
            .HasForeignKey(c => c.IdEstadoCita)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaginaSeo>()
            .HasIndex(p => p.Clave)
            .IsUnique();

        modelBuilder.Entity<Redireccion>()
            .HasIndex(r => r.RutaOrigen)
            .IsUnique();

        modelBuilder.Entity<ServicioContenido>()
            .HasOne(c => c.ServicioMedico)
            .WithMany(s => s.Contenidos)
            .HasForeignKey(c => c.IdServicioMedico)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ArticuloBlog>()
            .HasOne(a => a.ServicioMedico)
            .WithMany()
            .HasForeignKey(a => a.IdServicioMedico)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Testimonio>()
            .HasOne(t => t.ServicioMedico)
            .WithMany()
            .HasForeignKey(t => t.IdServicioMedico)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.IdRol)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ArticuloBlog>()
            .HasOne(a => a.Categoria)
            .WithMany(c => c.Articulos)
            .HasForeignKey(a => a.IdCategoria)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
