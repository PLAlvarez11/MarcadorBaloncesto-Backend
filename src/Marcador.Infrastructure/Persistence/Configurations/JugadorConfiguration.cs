using Marcador.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marcador.Infrastructure.Persistence.Configurations;

public class JugadorConfiguration : IEntityTypeConfiguration<Jugador>
{
    public void Configure(EntityTypeBuilder<Jugador> builder)
    {
        builder.ToTable("Jugadores");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NombreCompleto)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.Dorsal)
               .IsRequired();

        builder.Property(x => x.Estatura)
               .HasPrecision(4, 2); 

        builder.Property(x => x.Nacionalidad)
               .HasMaxLength(80);

    }
}
