using System.Linq.Expressions;
using Surgeon.Domain.Common;

namespace Surgeon.Application.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    /// <summary>Borrado lógico (Eliminado = 1).</summary>
    void Remove(T entity);
    /// <summary>Borrado físico: solo para datos auxiliares (bloques de contenido, redirecciones).</summary>
    void RemovePermanently(T entity);
}
