using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class DukkanMusteriConfiguration : IEntityTypeConfiguration<DukkanMusteri>
{
    public void Configure(EntityTypeBuilder<DukkanMusteri> builder)
    {
        builder.ToTable("tb_dukkan_musteri");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.DukkanId).HasColumnName("dukkan_id").IsRequired();
        builder.Property(x => x.MusteriId).HasColumnName("musteri_id").IsRequired();
        builder.Property(x => x.IlkZiyaretTarihi).HasColumnName("ilk_ziyaret_tarihi").HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.Notlar).HasColumnName("notlar");
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.Dukkan)
            .WithMany(x => x.DukkanMusterileri)
            .HasForeignKey(x => x.DukkanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Musteri)
            .WithMany(x => x.DukkanMusterileri)
            .HasForeignKey(x => x.MusteriId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}