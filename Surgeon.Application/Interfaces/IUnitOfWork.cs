using Surgeon.Domain.Entities;

namespace Surgeon.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<ServicioMedico> ServiciosMedicos { get; }
    IRepository<Especialidad> Especialidades { get; }
    IRepository<Cita> Citas { get; }
    IRepository<Paciente> Pacientes { get; }
    IRepository<ArticuloBlog> ArticulosBlog { get; }
    IRepository<Testimonio> Testimonios { get; }
    IRepository<MensajeContacto> MensajesContacto { get; }
    Task<int> SaveChangesAsync();
}
