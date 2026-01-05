namespace TeknikServis.Domain.Requests.Dukkan;

public record UpdateDukkanRequest(
    Guid Id,
    string Ad,
    string? Telefon,
    string? Email,
    string? Adres,
    string? VergiNo,
    bool Aktif,
    DateTime? AbonelikBitisTarihi
);