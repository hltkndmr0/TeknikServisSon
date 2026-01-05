using TeknikServis.Domain.Common;
using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Entities;

public class Teklif : BaseEntity
{
    public string TeklifNo { get; set; } = null!;
    public Guid IsEmriId { get; set; }
    public TeklifDurumu Durum { get; set; } = TeklifDurumu.Bekliyor;
    public decimal ToplamTutar { get; set; }
    public string? Aciklama { get; set; }
    public DateTime? GecerlilikTarihi { get; set; }
    public DateTime? OnayTarihi { get; set; }
    public bool MailGonderildi { get; set; } = false;
    public DateTime? MailGonderimTarihi { get; set; }
    public Guid OlusturanKullaniciId { get; set; }

    // Navigation
    public IsEmri IsEmri { get; set; } = null!;
    public Kullanici OlusturanKullanici { get; set; } = null!;
    public ICollection<TeklifKalem> Kalemler { get; set; } = new List<TeklifKalem>();
}