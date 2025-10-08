using Marcador.Domain.Common;
using Marcador.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marcador.Infrastructure.Persistence;

public class MarcadorDbContext : DbContext
{
    public MarcadorDbContext(DbContextOptions<MarcadorDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Acceso> Accesos => Set<Acceso>();
    public DbSet<RolAcceso> RolAccesos => Set<RolAcceso>();
    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Jugador> Jugadores => Set<Jugador>();
    public DbSet<Partido> Partidos => Set<Partido>();
    public DbSet<Logo> Logos => Set<Logo>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();    
    public DbSet<EstadisticaJugadorPartido> EstadisticaJugadorPartido => Set<EstadisticaJugadorPartido>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MarcadorDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.FechaCreacion = now;
                entry.Entity.FechaEdicion = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.FechaEdicion = now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
