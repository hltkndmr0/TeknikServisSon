using TeknikServis.Domain.Common;

namespace TeknikServis.Domain.Entities;

public class IsDurumu : BaseEntity
{
    public string Ad { get; set; } = null!;
    public string? Renk { get; set; }
    public int Sira { get; set; }
    public bool Aktif { get; set; } = true;
}