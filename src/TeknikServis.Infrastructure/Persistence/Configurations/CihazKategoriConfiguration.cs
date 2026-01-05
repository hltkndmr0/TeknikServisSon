using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class CihazKategoriConfiguration : IEntityTypeConfiguration<CihazKategori>
{
    public void Configure(EntityTypeBuilder<CihazKategori> builder)
    {
        builder.ToTable("tb_cihaz_kategori");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Ad).HasColumnName("ad").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Aktif).HasColumnName("aktif").HasDefaultValue(true);
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");
    }
}