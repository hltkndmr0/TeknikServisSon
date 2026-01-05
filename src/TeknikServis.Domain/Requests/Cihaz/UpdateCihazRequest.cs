namespace TeknikServis.Domain.Requests.Cihaz;

public record UpdateCihazRequest(
    Guid Id,
    string? SeriNo,
    string? Imei,
    string? Renk,
    DateTime? GarantiBitisTarihi,
    string? Notlar
);