using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Enums;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class KayitTalepConfiguration : IEntityTypeConfiguration<KayitTalep>
{
    public void Configure(EntityTypeBuilder<KayitTalep> builder)
    {
        builder.ToTable("tb_kayit_talep");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.FirmaAdi).HasColumnName("firma_adi").HasMaxLength(200).IsRequired();
        builder.Property(x => x.FirmaTelefon).HasColumnName("firma_telefon").HasMaxLength(20).IsRequired();
        builder.Property(x => x.FirmaEmail).HasColumnName("firma_email").HasMaxLength(100).IsRequired();
        builder.Property(x => x.FirmaAdres).HasColumnName("firma_adres").HasMaxLength(500);
        builder.Property(x => x.VergiNo).HasColumnName("vergi_no").HasMaxLength(20);
        builder.Property(x => x.YetkiliAdSoyad).HasColumnName("yetkili_ad_soyad").HasMaxLength(200).IsRequired();
        builder.Property(x => x.YetkiliTelefon).HasColumnName("yetkili_telefon").HasMaxLength(20).IsRequired();
        builder.Property(x => x.YetkiliEmail).HasColumnName("yetkili_email").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Durum).HasColumnName("durum").HasMaxLength(20).HasConversion<string>().HasDefaultValue(KayitTalepDurumu.Beklemede);
        builder.Property(x => x.RedNedeni).HasColumnName("red_nedeni").HasMaxLength(500);
        builder.Property(x => x.IslemYapanKullaniciId).HasColumnName("islem_yapan_kullanici_id");
        builder.Property(x => x.IslemTarihi).HasColumnName("islem_tarihi");
        builder.Property(x => x.Notlar).HasColumnName("notlar");
        builder.Property(x => x.TalepTarihi).HasColumnName("talep_tarihi").HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");
        
        // Yeni eklenen alanlar
        builder.Property(x => x.OlusturulanFirmaKodu).HasColumnName("olusturulan_firma_kodu").HasMaxLength(20);
        builder.Property(x => x.OlusturulanSifre).HasColumnName("olusturulan_sifre").HasMaxLength(100);

        builder.HasOne(x => x.IslemYapanKullanici)
            .WithMany()
            .HasForeignKey(x => x.IslemYapanKullaniciId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}