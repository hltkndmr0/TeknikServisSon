using TeknikServis.Domain.Common;

namespace TeknikServis.Domain.Entities;

public class Dukkan : BaseEntity
{
    public string Ad { get; set; } = null!;
    public string? Telefon { get; set; }
    public string? Email { get; set; }
    public string? Adres { get; set; }
    public string? VergiNo { get; set; }
    public string? FirmaKodu { get; set; }
    public bool Aktif { get; set; } = true;
    public DateTime? AbonelikBitisTarihi { get; set; }

    // Navigation
    public ICollection<Kullanici> Kullanicilar { get; set; } = new List<Kullanici>();
    public ICollection<DukkanMusteri> DukkanMusterileri { get; set; } = new List<DukkanMusteri>();
    public ICollection<IsEmri> IsEmirleri { get; set; } = new List<IsEmri>();
}