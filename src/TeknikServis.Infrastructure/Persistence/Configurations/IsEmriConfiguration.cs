using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Enums;

namespace TeknikServis.Infrastructure.Persistence.Configurations;

public class IsEmriConfiguration : IEntityTypeConfiguration<IsEmri>
{
    public void Configure(EntityTypeBuilder<IsEmri> builder)
    {
        builder.ToTable("tb_is_emri");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.IsEmriNo).HasColumnName("is_emri_no").HasMaxLength(50).IsRequired();
        builder.Property(x => x.DukkanId).HasColumnName("dukkan_id").IsRequired();
        builder.Property(x => x.MusteriId).HasColumnName("musteri_id").IsRequired();
        builder.Property(x => x.CihazId).HasColumnName("cihaz_id").IsRequired();
        builder.Property(x => x.DurumId).HasColumnName("durum_id").IsRequired();
        builder.Property(x => x.TeslimAlanKullaniciId).HasColumnName("teslim_alan_kullanici_id").IsRequired();
        builder.Property(x => x.AtananKullaniciId).HasColumnName("atanan_kullanici_id");
        builder.Property(x => x.ArizaAciklamasi).HasColumnName("ariza_aciklamasi");
        builder.Property(x => x.OnTeshis).HasColumnName("on_teshis");
        builder.Property(x => x.YapilanIslem).HasColumnName("yapilan_islem");
        builder.Property(x => x.TahminiSureGun).HasColumnName("tahmini_sure_gun");
        builder.Property(x => x.TahminiUcret).HasColumnName("tahmini_ucret").HasColumnType("decimal(18,2)");
        builder.Property(x => x.KesinUcret).HasColumnName("kesin_ucret").HasColumnType("decimal(18,2)");
        builder.Property(x => x.GarantiKapsaminda).HasColumnName("garanti_kapsaminda").HasDefaultValue(false);
        builder.Property(x => x.Oncelik).HasColumnName("oncelik").HasMaxLength(20).HasConversion<string>().HasDefaultValue(Oncelik.Normal);
        builder.Property(x => x.TeslimTarihi).HasColumnName("teslim_tarihi");
        builder.Property(x => x.TamamlanmaTarihi).HasColumnName("tamamlanma_tarihi");
        builder.Property(x => x.OlusturmaTarihi).HasColumnName("olusturma_tarihi").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(x => new { x.DukkanId, x.IsEmriNo }).IsUnique();

        builder.HasOne(x => x.Dukkan)
            .WithMany(x => x.IsEmirleri)
            .HasForeignKey(x => x.DukkanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Musteri)
            .WithMany()
            .HasForeignKey(x => x.MusteriId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Cihaz)
            .WithMany(x => x.IsEmirleri)
            .HasForeignKey(x => x.CihazId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Durum)
            .WithMany()
            .HasForeignKey(x => x.DurumId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TeslimAlanKullanici)
            .WithMany()
            .HasForeignKey(x => x.TeslimAlanKullaniciId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AtananKullanici)
            .WithMany()
            .HasForeignKey(x => x.AtananKullaniciId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}