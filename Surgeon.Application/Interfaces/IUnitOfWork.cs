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
    IRepository<Usuario> Usuarios { get; }
    IRepository<Rol> Roles { get; }
    IRepository<EstadoCita> EstadosCita { get; }
    IRepository<CategoriaBlog> CategoriasBlog { get; }
    IRepository<HorarioAtencion> HorariosAtencion { get; }
    IRepository<ServicioContenido> ServiciosContenido { get; }
    IRepository<ConfiguracionSitio> ConfiguracionSitio { get; }
    IRepository<PaginaSeo> PaginasSeo { get; }
    IRepository<Redireccion> Redirecciones { get; }
    IRepository<PaginaLegal> PaginasLegales { get; }
    Task<int> SaveChangesAsync();
}
