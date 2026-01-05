namespace TeknikServis.Domain.Requests.Musteri;

public record SearchMusteriRequest(
    string? Telefon,
    string? AdSoyad,
    string? FirmaAdi,
    int Page = 1,
    int PageSize = 20
);