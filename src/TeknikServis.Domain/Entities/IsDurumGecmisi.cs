using TeknikServis.Domain.Common;

namespace TeknikServis.Domain.Entities;

public class IsDurumGecmisi : BaseEntity
{
    public Guid IsEmriId { get; set; }
    public Guid DurumId { get; set; }
    public Guid KullaniciId { get; set; }
    public string? Aciklama { get; set; }
    public DateTime DegisimTarihi { get; set; } = DateTime.UtcNow;

    // Navigation
    public IsEmri IsEmri { get; set; } = null!;
    public IsDurumu Durum { get; set; } = null!;
    public Kullanici Kullanici { get; set; } = null!;
}