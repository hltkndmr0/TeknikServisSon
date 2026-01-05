namespace TeknikServis.Domain.Responses.Dukkan;

public record DukkanResponse(
    Guid Id,
    string Ad,
    string? Telefon,
    string? Email,
    string? Adres,
    string? VergiNo,
    string? FirmaKodu,
    bool Aktif,
    DateTime? AbonelikBitisTarihi,
    DateTime OlusturmaTarihi
);