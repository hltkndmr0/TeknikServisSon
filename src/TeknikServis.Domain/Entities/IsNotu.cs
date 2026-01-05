using TeknikServis.Domain.Common;

namespace TeknikServis.Domain.Entities;

public class IsNotu : BaseEntity
{
    public Guid IsEmriId { get; set; }
    public Guid KullaniciId { get; set; }
    public string NotMetni { get; set; } = null!;

    // Navigation
    public IsEmri IsEmri { get; set; } = null!;
    public Kullanici Kullanici { get; set; } = null!;
}