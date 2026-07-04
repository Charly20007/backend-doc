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
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
