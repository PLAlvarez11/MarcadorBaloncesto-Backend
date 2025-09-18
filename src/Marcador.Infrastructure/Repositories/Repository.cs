using System.Linq.Expressions;
using Marcador.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Marcador.Infrastructure.Persistence;

namespace Marcador.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly MarcadorDbContext _db;

    public Repository(MarcadorDbContext db) => _db = db;

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Set<T>().FindAsync(new object?[] { id }, ct);

    public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? filter = null, CancellationToken ct = default)
    {
        IQueryable<T> query = _db.Set<T>();
        if (filter != null) query = query.Where(filter);
        return await query.AsNoTracking().ToListAsync(ct);
    }

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await _db.Set<T>().AddAsync(entity, ct);

    public void Update(T entity)
        => _db.Set<T>().Update(entity);

    public void Delete(T entity)
        => _db.Set<T>().Remove(entity);
}
