using TeknikServis.Domain.Common;

namespace TeknikServis.Domain.Entities;

public class Fatura : BaseEntity
{
    public string FaturaNo { get; set; } = null!;
    public Guid IsEmriId { get; set; }
    public Guid? TeklifId { get; set; }
    public DateTime FaturaTarihi { get; set; } = DateTime.UtcNow;
    public decimal ToplamTutar { get; set; }
    public int KdvOrani { get; set; } = 20;
    public decimal KdvTutar { get; set; }
    public decimal GenelToplam { get; set; }
    public string? Aciklama { get; set; }
    public Guid OlusturanKullaniciId { get; set; }

    // Navigation
    public IsEmri IsEmri { get; set; } = null!;
    public Teklif? Teklif { get; set; }
    public Kullanici OlusturanKullanici { get; set; } = null!;
    public ICollection<FaturaKalem> Kalemler { get; set; } = new List<FaturaKalem>();
}