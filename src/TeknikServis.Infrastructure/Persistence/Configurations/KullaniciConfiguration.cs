using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class KullaniciConfiguration : IEntityTypeConfiguration<Kullanici>
{
    public void Configure(EntityTypeBuilder<Kullanici> builder)
    {
        builder.ToTable("tb_kullanici");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.DukkanId).HasColumnName("dukkan_id");
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
        builder.Property(x => x.SifreHash).HasColumnName("sifre_hash").HasMaxLength(500).IsRequired();
        builder.Property(x => x.AdSoyad).HasColumnName("ad_soyad").HasMaxLength(200).IsRequired();
        builder.Property(x => x.Telefon).HasColumnName("telefon").HasMaxLength(20);
        builder.Property(x => x.Rol).HasColumnName("rol").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.Aktif).HasColumnName("aktif").HasDefaultValue(true);
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasOne(x => x.Dukkan)
            .WithMany(x => x.Kullanicilar)
            .HasForeignKey(x => x.DukkanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}