using Marcador.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marcador.Infrastructure.Persistence.Configurations;

public class EquipoConfiguration : IEntityTypeConfiguration<Equipo>
{
    public void Configure(EntityTypeBuilder<Equipo> builder)
    {
        builder.ToTable("Equipos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nombre)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(x => x.Nombre)
               .IsUnique();

        builder.Property(x => x.Ciudad)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasMany(e => e.Jugadores)
               .WithOne(j => j.Equipo)
               .HasForeignKey(j => j.EquipoId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Logo)
               .WithOne(l => l.Equipo)
               .HasForeignKey<Logo>(l => l.EquipoId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
