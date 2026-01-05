using TeknikServis.Domain.Common;
using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Entities;

public class KayitTalep : BaseEntity
{
    // Firma Bilgileri
    public string FirmaAdi { get; set; } = null!;
    public string FirmaTelefon { get; set; } = null!;
    public string FirmaEmail { get; set; } = null!;
    public string? FirmaAdres { get; set; }
    public string? VergiNo { get; set; }

    // Yetkili Kişi Bilgileri
    public string YetkiliAdSoyad { get; set; } = null!;
    public string YetkiliTelefon { get; set; } = null!;
    public string YetkiliEmail { get; set; } = null!;

    // Talep Durumu
    public KayitTalepDurumu Durum { get; set; } = KayitTalepDurumu.Beklemede;
    public string? RedNedeni { get; set; }

    // İşlem Bilgileri
    public Guid? IslemYapanKullaniciId { get; set; }
    public DateTime? IslemTarihi { get; set; }
    public string? Notlar { get; set; }
    public DateTime TalepTarihi { get; set; } = DateTime.UtcNow;

    // Onay sonrası oluşturulan bilgiler
    public string? OlusturulanFirmaKodu { get; set; }
    public string? OlusturulanSifre { get; set; }

    // Navigation
    public Kullanici? IslemYapanKullanici { get; set; }
}