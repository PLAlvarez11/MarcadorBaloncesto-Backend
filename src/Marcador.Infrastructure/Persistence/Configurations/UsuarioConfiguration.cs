using Marcador.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marcador.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NombreUsuario)
               .IsRequired()
               .HasMaxLength(64);

        builder.HasIndex(x => x.NombreUsuario)
               .IsUnique();

        builder.Property(x => x.PasswordHash)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasOne(x => x.Rol)
               .WithMany(r => r.Usuarios)
               .HasForeignKey(x => x.RolId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
