using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class MusteriConfiguration : IEntityTypeConfiguration<Musteri>
{
    public void Configure(EntityTypeBuilder<Musteri> builder)
    {
        builder.ToTable("tb_musteri");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.MusteriTipi).HasColumnName("musteri_tipi").HasMaxLength(20).HasConversion<string>().IsRequired();
        builder.Property(x => x.AdSoyad).HasColumnName("ad_soyad").HasMaxLength(200);
        builder.Property(x => x.FirmaAdi).HasColumnName("firma_adi").HasMaxLength(200);
        builder.Property(x => x.VergiNo).HasColumnName("vergi_no").HasMaxLength(20);
        builder.Property(x => x.Telefon).HasColumnName("telefon").HasMaxLength(20).IsRequired();
        builder.Property(x => x.Telefon2).HasColumnName("telefon2").HasMaxLength(20);
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(100);
        builder.Property(x => x.Adres).HasColumnName("adres").HasMaxLength(500);
        builder.Property(x => x.Notlar).HasColumnName("notlar");
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");
    }
}