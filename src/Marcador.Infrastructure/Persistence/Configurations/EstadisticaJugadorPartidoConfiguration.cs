using Marcador.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marcador.Infrastructure.Persistence.Configurations;

public class EstadisticaJugadorPartidoConfiguration : IEntityTypeConfiguration<EstadisticaJugadorPartido>
{
    public void Configure(EntityTypeBuilder<EstadisticaJugadorPartido> builder)
    {
        builder.ToTable("EstadisticaJugadorPartido");

        builder.HasKey(x => new { x.PartidoId, x.JugadorId });

        builder.Property(x => x.Puntos).HasDefaultValue(0);
        builder.Property(x => x.Asistencias).HasDefaultValue(0);
        builder.Property(x => x.RebotesOf).HasDefaultValue(0);
        builder.Property(x => x.RebotesDef).HasDefaultValue(0);
        builder.Property(x => x.Robos).HasDefaultValue(0);
        builder.Property(x => x.Tapones).HasDefaultValue(0);
        builder.Property(x => x.Faltas).HasDefaultValue(0);

        builder.HasOne(x => x.Partido)
            .WithMany()
            .HasForeignKey(x => x.PartidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Jugador)
            .WithMany() 
            .HasForeignKey(x => x.JugadorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
