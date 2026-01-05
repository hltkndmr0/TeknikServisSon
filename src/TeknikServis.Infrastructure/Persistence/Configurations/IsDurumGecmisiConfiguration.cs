using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class IsDurumGecmisiConfiguration : IEntityTypeConfiguration<IsDurumGecmisi>
{
    public void Configure(EntityTypeBuilder<IsDurumGecmisi> builder)
    {
        builder.ToTable("tb_is_durum_gecmisi");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.IsEmriId).HasColumnName("is_emri_id").IsRequired();
        builder.Property(x => x.DurumId).HasColumnName("durum_id").IsRequired();
        builder.Property(x => x.KullaniciId).HasColumnName("kullanici_id").IsRequired();
        builder.Property(x => x.Aciklama).HasColumnName("aciklama").HasMaxLength(500);
        builder.Property(x => x.DegisimTarihi).HasColumnName("degisim_tarihi").HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.IsEmri)
            .WithMany(x => x.DurumGecmisi)
            .HasForeignKey(x => x.IsEmriId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Durum)
            .WithMany()
            .HasForeignKey(x => x.DurumId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Kullanici)
            .WithMany()
            .HasForeignKey(x => x.KullaniciId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}