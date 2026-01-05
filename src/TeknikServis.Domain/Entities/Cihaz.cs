using TeknikServis.Domain.Common;

namespace TeknikServis.Domain.Entities;

public class Cihaz : BaseEntity
{
    public Guid CihazTanimId { get; set; }
    public Guid MusteriId { get; set; }
    public string? SeriNo { get; set; }
    public string? Imei { get; set; }
    public string? Renk { get; set; }
    public DateTime? GarantiBitisTarihi { get; set; }
    public string? Notlar { get; set; }

    // Navigation
    public CihazTanim CihazTanim { get; set; } = null!;
    public Musteri Musteri { get; set; } = null!;
    public ICollection<IsEmri> IsEmirleri { get; set; } = new List<IsEmri>();
}