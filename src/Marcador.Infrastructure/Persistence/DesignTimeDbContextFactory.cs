using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Marcador.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MarcadorDbContext>
{
    public MarcadorDbContext CreateDbContext(string[] args)
    {
        // Usa variable de entorno o fallback local
        var conn = Environment.GetEnvironmentVariable("MARCADOR_SQLSERVER")
                  ?? "Server=localhost,1433;Database=MarcadorDb;User Id=sa;Password=Baloncesto11!;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<MarcadorDbContext>();
        optionsBuilder.UseSqlServer(conn);

        return new MarcadorDbContext(optionsBuilder.Options);
    }
}
