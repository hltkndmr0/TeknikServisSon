namespace TeknikServis.Domain.Requests.Teklif;

public record ReddetTeklifRequest(
    Guid TeklifId,
    string? RedNedeni
);