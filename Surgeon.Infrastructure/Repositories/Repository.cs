using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Surgeon.Application.Interfaces;
using Surgeon.Domain.Common;
using Surgeon.Infrastructure.Persistence;

namespace Surgeon.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly SurgeonDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(SurgeonDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.Eliminado);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.Where(e => !e.Eliminado).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).Where(e => !e.Eliminado).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        entity.FechaModificacion = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        // Borrado lógico estándar
        entity.Eliminado = true;
        entity.FechaModificacion = DateTime.UtcNow;
        _dbSet.Update(entity);
    }
}
