using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class DukkanConfiguration : IEntityTypeConfiguration<Dukkan>
{
    public void Configure(EntityTypeBuilder<Dukkan> builder)
    {
        builder.ToTable("tb_dukkan");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Ad).HasColumnName("ad").HasMaxLength(200).IsRequired();
        builder.Property(x => x.Telefon).HasColumnName("telefon").HasMaxLength(20);
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(100);
        builder.Property(x => x.Adres).HasColumnName("adres").HasMaxLength(500);
        builder.Property(x => x.VergiNo).HasColumnName("vergi_no").HasMaxLength(20);
        builder.Property(x => x.FirmaKodu).HasColumnName("firma_kodu").HasMaxLength(10);
        builder.Property(x => x.Aktif).HasColumnName("aktif").HasDefaultValue(true);
        builder.Property(x => x.AbonelikBitisTarihi).HasColumnName("abonelik_bitis_tarihi");
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(x => x.FirmaKodu).IsUnique().HasFilter("[firma_kodu] IS NOT NULL");
    }
}