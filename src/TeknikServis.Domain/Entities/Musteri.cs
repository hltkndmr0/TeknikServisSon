using TeknikServis.Domain.Common;
using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Entities;

public class Musteri : BaseEntity
{
    public MusteriTipi MusteriTipi { get; set; }
    public string? AdSoyad { get; set; }
    public string? FirmaAdi { get; set; }
    public string? VergiNo { get; set; }
    public string Telefon { get; set; } = null!;
    public string? Telefon2 { get; set; }
    public string? Email { get; set; }
    public string? Adres { get; set; }
    public string? Notlar { get; set; }

    // Navigation
    public ICollection<DukkanMusteri> DukkanMusterileri { get; set; } = new List<DukkanMusteri>();
    public ICollection<Cihaz> Cihazlar { get; set; } = new List<Cihaz>();
}