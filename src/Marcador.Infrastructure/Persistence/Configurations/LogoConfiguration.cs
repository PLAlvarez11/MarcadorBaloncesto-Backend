using Marcador.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marcador.Infrastructure.Persistence.Configurations;

public class LogoConfiguration : IEntityTypeConfiguration<Logo>
{
    public void Configure(EntityTypeBuilder<Logo> builder)
    {
        builder.ToTable("Logos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileType)
               .HasMaxLength(64)
               .IsRequired();

        builder.Property(x => x.FileData)
               .IsRequired();
    }
}
