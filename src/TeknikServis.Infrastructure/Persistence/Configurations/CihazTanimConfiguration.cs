using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class CihazTanimConfiguration : IEntityTypeConfiguration<CihazTanim>
{
    public void Configure(EntityTypeBuilder<CihazTanim> builder)
    {
        builder.ToTable("tb_cihaz_tanim");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.KategoriId).HasColumnName("kategori_id").IsRequired();
        builder.Property(x => x.Marka).HasColumnName("marka").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Model).HasColumnName("model").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Aktif).HasColumnName("aktif").HasDefaultValue(true);
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.Kategori)
            .WithMany(x => x.CihazTanimlari)
            .HasForeignKey(x => x.KategoriId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}