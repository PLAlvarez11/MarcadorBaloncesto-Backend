using Marcador.Domain.Interfaces;
using Marcador.Infrastructure.Persistence;

namespace Marcador.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MarcadorDbContext _db;
    public UnitOfWork(MarcadorDbContext db) => _db = db;

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
