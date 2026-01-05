namespace TeknikServis.Domain.Requests.Cihaz;

public record CreateCihazRequest(
    Guid CihazTanimId,
    Guid MusteriId,
    string? SeriNo,
    string? Imei,
    string? Renk,
    DateTime? GarantiBitisTarihi,
    string? Notlar
);