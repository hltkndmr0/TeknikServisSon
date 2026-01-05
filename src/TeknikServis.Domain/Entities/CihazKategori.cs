using TeknikServis.Domain.Common;

namespace TeknikServis.Domain.Entities;

public class CihazKategori : BaseEntity
{
    public string Ad { get; set; } = null!;
    public bool Aktif { get; set; } = true;

    // Navigation
    public ICollection<CihazTanim> CihazTanimlari { get; set; } = new List<CihazTanim>();
}