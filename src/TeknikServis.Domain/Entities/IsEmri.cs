using TeknikServis.Domain.Common;
using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Entities;

public class IsEmri : BaseEntity
{
    public string IsEmriNo { get; set; } = null!;
    public Guid DukkanId { get; set; }
    public Guid MusteriId { get; set; }
    public Guid CihazId { get; set; }
    public Guid DurumId { get; set; }
    public Guid TeslimAlanKullaniciId { get; set; }
    public Guid? AtananKullaniciId { get; set; }
    public string? ArizaAciklamasi { get; set; }
    public string? OnTeshis { get; set; }
    public string? YapilanIslem { get; set; }
    public int? TahminiSureGun { get; set; }
    public decimal? TahminiUcret { get; set; }
    public decimal? KesinUcret { get; set; }
    public bool GarantiKapsaminda { get; set; } = false;
    public Oncelik Oncelik { get; set; } = Oncelik.Normal;
    public DateTime? TeslimTarihi { get; set; }
    public DateTime? TamamlanmaTarihi { get; set; }

    // Navigation
    public Dukkan Dukkan { get; set; } = null!;
    public Musteri Musteri { get; set; } = null!;
    public Cihaz Cihaz { get; set; } = null!;
    public IsDurumu Durum { get; set; } = null!;
    public Kullanici TeslimAlanKullanici { get; set; } = null!;
    public Kullanici? AtananKullanici { get; set; }
    public ICollection<IsNotu> IsNotlari { get; set; } = new List<IsNotu>();
    public ICollection<IsDurumGecmisi> DurumGecmisi { get; set; } = new List<IsDurumGecmisi>();
    public ICollection<Teklif> Teklifler { get; set; } = new List<Teklif>();
    public ICollection<Fatura> Faturalar { get; set; } = new List<Fatura>();
}