using Marcador.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marcador.Infrastructure.Persistence.Configurations;

public class AccesoConfiguration : IEntityTypeConfiguration<Acceso>
{
    public void Configure(EntityTypeBuilder<Acceso> builder)
    {
        builder.ToTable("Accesos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NombreAcceso)
               .IsRequired()
               .HasMaxLength(64);

        builder.HasIndex(x => x.NombreAcceso)
               .IsUnique();
    }
}
