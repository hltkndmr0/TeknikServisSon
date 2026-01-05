using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class IsDurumuConfiguration : IEntityTypeConfiguration<IsDurumu>
{
    public void Configure(EntityTypeBuilder<IsDurumu> builder)
    {
        builder.ToTable("tb_is_durumu");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Ad).HasColumnName("ad").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Renk).HasColumnName("renk").HasMaxLength(20);
        builder.Property(x => x.Sira).HasColumnName("sira").HasDefaultValue(0);
        builder.Property(x => x.Aktif).HasColumnName("aktif").HasDefaultValue(true);
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");
    }
}