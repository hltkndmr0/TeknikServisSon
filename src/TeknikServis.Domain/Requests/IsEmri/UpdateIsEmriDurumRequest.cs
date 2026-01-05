namespace TeknikServis.Domain.Requests.IsEmri;

public record UpdateIsEmriDurumRequest(
    Guid IsEmriId,
    Guid DurumId,
    string? Aciklama
);