namespace TeknikServis.Domain.Responses.Cihaz;

public class CihazListResponse
{
    public Guid Id { get; set; }
    public Guid MusteriId { get; set; }
    public string MusteriAd { get; set; } = null!;
    public Guid CihazTanimId { get; set; }
    public string KategoriAd { get; set; } = null!;
    public string Marka { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? SeriNo { get; set; }
    public string? Imei { get; set; }
    public string? Renk { get; set; }
    public DateTime OlusturmaTarihi { get; set; }
}