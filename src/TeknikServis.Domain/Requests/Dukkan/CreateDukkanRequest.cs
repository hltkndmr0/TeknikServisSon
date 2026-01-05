namespace TeknikServis.Domain.Requests.Dukkan;

public record CreateDukkanRequest(
    string Ad,
    string? Telefon,
    string? Email,
    string? Adres,
    string? VergiNo,
    DateTime? AbonelikBitisTarihi
);