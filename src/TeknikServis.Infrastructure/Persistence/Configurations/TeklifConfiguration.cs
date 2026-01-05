using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Enums;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class TeklifConfiguration : IEntityTypeConfiguration<Teklif>
{
    public void Configure(EntityTypeBuilder<Teklif> builder)
    {
        builder.ToTable("tb_teklif");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.TeklifNo).HasColumnName("teklif_no").HasMaxLength(50).IsRequired();
        builder.Property(x => x.IsEmriId).HasColumnName("is_emri_id").IsRequired();
        builder.Property(x => x.Durum).HasColumnName("durum").HasMaxLength(20).HasConversion<string>().HasDefaultValue(TeklifDurumu.Bekliyor);
        builder.Property(x => x.ToplamTutar).HasColumnName("toplam_tutar").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.Aciklama).HasColumnName("aciklama");
        builder.Property(x => x.GecerlilikTarihi).HasColumnName("gecerlilik_tarihi");
        builder.Property(x => x.OnayTarihi).HasColumnName("onay_tarihi");
        builder.Property(x => x.MailGonderildi).HasColumnName("mail_gonderildi").HasDefaultValue(false);
        builder.Property(x => x.MailGonderimTarihi).HasColumnName("mail_gonderim_tarihi");
        builder.Property(x => x.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id").IsRequired();
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(x => new { x.IsEmriId, x.TeklifNo }).IsUnique();

        builder.HasOne(x => x.IsEmri)
            .WithMany(x => x.Teklifler)
            .HasForeignKey(x => x.IsEmriId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.OlusturanKullanici)
            .WithMany()
            .HasForeignKey(x => x.OlusturanKullaniciId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}