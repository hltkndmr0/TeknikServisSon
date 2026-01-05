using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class CihazConfiguration : IEntityTypeConfiguration<Cihaz>
{
    public void Configure(EntityTypeBuilder<Cihaz> builder)
    {
        builder.ToTable("tb_cihaz");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.CihazTanimId).HasColumnName("cihaz_tanim_id").IsRequired();
        builder.Property(x => x.MusteriId).HasColumnName("musteri_id").IsRequired();
        builder.Property(x => x.SeriNo).HasColumnName("seri_no").HasMaxLength(100);
        builder.Property(x => x.Imei).HasColumnName("imei").HasMaxLength(20);
        builder.Property(x => x.Renk).HasColumnName("renk").HasMaxLength(50);
        builder.Property(x => x.GarantiBitisTarihi).HasColumnName("garanti_bitis_tarihi");
        builder.Property(x => x.Notlar).HasColumnName("notlar");
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(x => x.SeriNo).IsUnique().HasFilter("[seri_no] IS NOT NULL");
        builder.HasIndex(x => x.Imei).IsUnique().HasFilter("[imei] IS NOT NULL");

        builder.HasOne(x => x.CihazTanim)
            .WithMany(x => x.Cihazlar)
            .HasForeignKey(x => x.CihazTanimId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Musteri)
            .WithMany(x => x.Cihazlar)
            .HasForeignKey(x => x.MusteriId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}