using TeknikServis.Domain.Common;

namespace TeknikServis.Domain.Entities;

public class CihazTanim : BaseEntity
{
    public Guid KategoriId { get; set; }
    public string Marka { get; set; } = null!;
    public string Model { get; set; } = null!;
    public bool Aktif { get; set; } = true;

    // Navigation
    public CihazKategori Kategori { get; set; } = null!;
    public ICollection<Cihaz> Cihazlar { get; set; } = new List<Cihaz>();
}