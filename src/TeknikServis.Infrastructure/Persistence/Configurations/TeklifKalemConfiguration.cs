using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class TeklifKalemConfiguration : IEntityTypeConfiguration<TeklifKalem>
{
    public void Configure(EntityTypeBuilder<TeklifKalem> builder)
    {
        builder.ToTable("tb_teklif_kalem");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.TeklifId).HasColumnName("teklif_id").IsRequired();
        builder.Property(x => x.KalemTipi).HasColumnName("kalem_tipi").HasMaxLength(20).HasConversion<string>().IsRequired();
        builder.Property(x => x.Aciklama).HasColumnName("aciklama").HasMaxLength(500).IsRequired();
        builder.Property(x => x.Miktar).HasColumnName("miktar").HasDefaultValue(1);
        builder.Property(x => x.BirimFiyat).HasColumnName("birim_fiyat").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.ToplamFiyat).HasColumnName("toplam_fiyat").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.Teklif)
            .WithMany(x => x.Kalemler)
            .HasForeignKey(x => x.TeklifId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}