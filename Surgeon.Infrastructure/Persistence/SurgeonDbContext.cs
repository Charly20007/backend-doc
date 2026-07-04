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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeo exacto con los nombres de tablas en 01_Tabla_Maestra.sql
        modelBuilder.Entity<Rol>().ToTable("Roles");
        modelBuilder.Entity<Usuario>().ToTable("Usuarios");
        modelBuilder.Entity<Especialidad>().ToTable("Especialidades");
        modelBuilder.Entity<ServicioMedico>().ToTable("ServiciosMedicos");
        modelBuilder.Entity<Paciente>().ToTable("Pacientes");
        modelBuilder.Entity<EstadoCita>().ToTable("EstadosCita");
        modelBuilder.Entity<Cita>().ToTable("Citas");
        modelBuilder.Entity<HorarioAtencion>().ToTable("HorariosAtencion");
        modelBuilder.Entity<BloqueoHorario>().ToTable("BloqueosHorario");
        modelBuilder.Entity<CategoriaBlog>().ToTable("CategoriasBlog");
        modelBuilder.Entity<ArticuloBlog>().ToTable("ArticulosBlog");
        modelBuilder.Entity<Testimonio>().ToTable("Testimonios");
        modelBuilder.Entity<MensajeContacto>().ToTable("MensajesContacto");

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

        modelBuilder.Entity<ArticuloBlog>()
            .HasOne(a => a.Categoria)
            .WithMany(c => c.Articulos)
            .HasForeignKey(a => a.IdCategoria)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
