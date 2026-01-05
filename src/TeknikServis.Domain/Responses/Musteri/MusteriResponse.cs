using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Responses.Musteri;

public record MusteriResponse(
    Guid Id,
    MusteriTipi MusteriTipi,
    string? AdSoyad,
    string? FirmaAdi,
    string? VergiNo,
    string Telefon,
    string? Telefon2,
    string? Email,
    string? Adres,
    string? Notlar,
    DateTime OlusturmaTarihi
);