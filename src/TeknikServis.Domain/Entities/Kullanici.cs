using TeknikServis.Domain.Common;
using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Entities;

public class Kullanici : BaseEntity
{
    public Guid? DukkanId { get; set; }
    public string Email { get; set; } = null!;
    public string SifreHash { get; set; } = null!;
    public string AdSoyad { get; set; } = null!;
    public string? Telefon { get; set; }
    public KullaniciRol Rol { get; set; }
    public bool Aktif { get; set; } = true;

    // Navigation
    public Dukkan? Dukkan { get; set; }
}