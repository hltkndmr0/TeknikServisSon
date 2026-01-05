using TeknikServis.Domain.Common;

namespace TeknikServis.Domain.Entities;

public class DukkanMusteri : BaseEntity
{
    public Guid DukkanId { get; set; }
    public Guid MusteriId { get; set; }
    public DateTime IlkZiyaretTarihi { get; set; } = DateTime.UtcNow;
    public string? Notlar { get; set; }

    // Navigation
    public Dukkan Dukkan { get; set; } = null!;
    public Musteri Musteri { get; set; } = null!;
}