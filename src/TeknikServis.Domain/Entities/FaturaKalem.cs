using TeknikServis.Domain.Common;
using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Entities;

public class FaturaKalem : BaseEntity
{
    public Guid FaturaId { get; set; }
    public KalemTipi KalemTipi { get; set; }
    public string Aciklama { get; set; } = null!;
    public int Miktar { get; set; } = 1;
    public decimal BirimFiyat { get; set; }
    public decimal ToplamFiyat { get; set; }

    // Navigation
    public Fatura Fatura { get; set; } = null!;
}