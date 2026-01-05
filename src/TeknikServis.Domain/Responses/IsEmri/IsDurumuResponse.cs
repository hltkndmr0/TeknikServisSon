namespace TeknikServis.Domain.Responses.IsEmri;

public record IsDurumuResponse(
    Guid Id,
    string Ad,
    string? Renk,
    int Sira
);