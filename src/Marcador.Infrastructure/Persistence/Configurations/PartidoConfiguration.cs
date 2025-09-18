using Marcador.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marcador.Infrastructure.Persistence.Configurations;

public class PartidoConfiguration : IEntityTypeConfiguration<Partido>
{
    public void Configure(EntityTypeBuilder<Partido> builder)
    {
        builder.ToTable("Partidos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PuntajeEquipo1).HasDefaultValue(0);
        builder.Property(x => x.PuntajeEquipo2).HasDefaultValue(0);
        builder.Property(x => x.FaltasEquipo1).HasDefaultValue(0);
        builder.Property(x => x.FaltasEquipo2).HasDefaultValue(0);
        builder.Property(x => x.CuartoActual).HasDefaultValue(1);

        builder.HasOne(p => p.Equipo1)
               .WithMany(e => e.PartidosComoLocal)
               .HasForeignKey(p => p.Equipo1Id)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Equipo2)
               .WithMany(e => e.PartidosComoVisitante)
               .HasForeignKey(p => p.Equipo2Id)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.FechaPartido);
    }
}
