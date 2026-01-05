namespace TeknikServis.Domain.Responses.Cihaz;

public record CihazResponse(
    Guid Id,
    Guid CihazTanimId,
    string Marka,
    string Model,
    string KategoriAd,
    Guid MusteriId,
    string? MusteriAd,
    string? SeriNo,
    string? Imei,
    string? Renk,
    DateTime? GarantiBitisTarihi,
    string? Notlar,
    DateTime OlusturmaTarihi
);