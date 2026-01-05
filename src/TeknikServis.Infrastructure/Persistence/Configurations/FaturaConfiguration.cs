using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class FaturaConfiguration : IEntityTypeConfiguration<Fatura>
{
    public void Configure(EntityTypeBuilder<Fatura> builder)
    {
        builder.ToTable("tb_fatura");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.FaturaNo).HasColumnName("fatura_no").HasMaxLength(50).IsRequired();
        builder.Property(x => x.IsEmriId).HasColumnName("is_emri_id").IsRequired();
        builder.Property(x => x.TeklifId).HasColumnName("teklif_id");
        builder.Property(x => x.FaturaTarihi).HasColumnName("fatura_tarihi").HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.ToplamTutar).HasColumnName("toplam_tutar").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.KdvOrani).HasColumnName("kdv_orani").HasDefaultValue(20);
        builder.Property(x => x.KdvTutar).HasColumnName("kdv_tutar").HasColumnType("decimal(18,2)");
        builder.Property(x => x.GenelToplam).HasColumnName("genel_toplam").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.Aciklama).HasColumnName("aciklama");
        builder.Property(x => x.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id").IsRequired();
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.IsEmri)
            .WithMany(x => x.Faturalar)
            .HasForeignKey(x => x.IsEmriId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Teklif)
            .WithMany()
            .HasForeignKey(x => x.TeklifId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.OlusturanKullanici)
            .WithMany()
            .HasForeignKey(x => x.OlusturanKullaniciId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}