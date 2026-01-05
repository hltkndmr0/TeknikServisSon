using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Requests.Musteri;

public record CreateMusteriRequest(
    MusteriTipi MusteriTipi,
    string? AdSoyad,
    string? FirmaAdi,
    string? VergiNo,
    string Telefon,
    string? Telefon2,
    string? Email,
    string? Adres,
    string? Notlar
);