using Microsoft.EntityFrameworkCore;
using Npgsql;
using Surgeon.Application.Common;
using Surgeon.Application.Interfaces;
using Surgeon.Domain.Entities;
using Surgeon.Infrastructure.Persistence;

namespace Surgeon.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SurgeonDbContext _context;

    public IRepository<ServicioMedico> ServiciosMedicos { get; }
    public IRepository<Especialidad> Especialidades { get; }
    public IRepository<Cita> Citas { get; }
    public IRepository<Paciente> Pacientes { get; }
    public IRepository<ArticuloBlog> ArticulosBlog { get; }
    public IRepository<Testimonio> Testimonios { get; }
    public IRepository<MensajeContacto> MensajesContacto { get; }
    public IRepository<Usuario> Usuarios { get; }
    public IRepository<Rol> Roles { get; }
    public IRepository<EstadoCita> EstadosCita { get; }
    public IRepository<CategoriaBlog> CategoriasBlog { get; }
    public IRepository<HorarioAtencion> HorariosAtencion { get; }
    public IRepository<BloqueoHorario> BloqueosHorario { get; }
    public IRepository<ServicioContenido> ServiciosContenido { get; }
    public IRepository<ConfiguracionSitio> ConfiguracionSitio { get; }
    public IRepository<PaginaSeo> PaginasSeo { get; }
    public IRepository<Redireccion> Redirecciones { get; }
    public IRepository<PaginaLegal> PaginasLegales { get; }

    public UnitOfWork(SurgeonDbContext context)
    {
        _context = context;
        ServiciosMedicos = new Repository<ServicioMedico>(_context);
        Especialidades = new Repository<Especialidad>(_context);
        Citas = new Repository<Cita>(_context);
        Pacientes = new Repository<Paciente>(_context);
        ArticulosBlog = new Repository<ArticuloBlog>(_context);
        Testimonios = new Repository<Testimonio>(_context);
        MensajesContacto = new Repository<MensajeContacto>(_context);
        Usuarios = new Repository<Usuario>(_context);
        Roles = new Repository<Rol>(_context);
        EstadosCita = new Repository<EstadoCita>(_context);
        CategoriasBlog = new Repository<CategoriaBlog>(_context);
        HorariosAtencion = new Repository<HorarioAtencion>(_context);
        BloqueosHorario = new Repository<BloqueoHorario>(_context);
        ServiciosContenido = new Repository<ServicioContenido>(_context);
        ConfiguracionSitio = new Repository<ConfiguracionSitio>(_context);
        PaginasSeo = new Repository<PaginaSeo>(_context);
        Redirecciones = new Repository<Redireccion>(_context);
        PaginasLegales = new Repository<PaginaLegal>(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            // Se traduce a una excepción propia para que Application no dependa de EF ni de PostgreSQL
            _context.ChangeTracker.Clear();
            throw new ConflictoDeDatosException("Los datos entran en conflicto con un registro existente.", ex);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
