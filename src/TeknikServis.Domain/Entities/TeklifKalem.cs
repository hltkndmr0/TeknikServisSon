using TeknikServis.Domain.Common;
using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Entities;

public class TeklifKalem : BaseEntity
{
    public Guid TeklifId { get; set; }
    public KalemTipi KalemTipi { get; set; }
    public string Aciklama { get; set; } = null!;
    public int Miktar { get; set; } = 1;
    public decimal BirimFiyat { get; set; }
    public decimal ToplamFiyat { get; set; }

    // Navigation
    public Teklif Teklif { get; set; } = null!;
}