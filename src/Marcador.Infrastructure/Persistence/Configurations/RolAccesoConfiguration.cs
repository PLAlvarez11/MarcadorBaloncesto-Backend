using Marcador.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marcador.Infrastructure.Persistence.Configurations;

public class RolAccesoConfiguration : IEntityTypeConfiguration<RolAcceso>
{
    public void Configure(EntityTypeBuilder<RolAcceso> builder)
    {
        builder.ToTable("RolAccesos");
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Rol)
               .WithMany(r => r.RolAccesos)
               .HasForeignKey(x => x.RolId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Acceso)
               .WithMany(a => a.RolAccesos)
               .HasForeignKey(x => x.AccesoId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.RolId, x.AccesoId })
               .IsUnique();
    }
}
