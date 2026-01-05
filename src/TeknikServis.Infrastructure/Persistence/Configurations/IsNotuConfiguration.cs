using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class IsNotuConfiguration : IEntityTypeConfiguration<IsNotu>
{
    public void Configure(EntityTypeBuilder<IsNotu> builder)
    {
        builder.ToTable("tb_is_notu");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.IsEmriId).HasColumnName("is_emri_id").IsRequired();
        builder.Property(x => x.KullaniciId).HasColumnName("kullanici_id").IsRequired();
        builder.Property(x => x.NotMetni).HasColumnName("not_metni").IsRequired();
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.IsEmri)
            .WithMany(x => x.IsNotlari)
            .HasForeignKey(x => x.IsEmriId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Kullanici)
            .WithMany()
            .HasForeignKey(x => x.KullaniciId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}