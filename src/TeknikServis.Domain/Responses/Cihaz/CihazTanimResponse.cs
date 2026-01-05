namespace TeknikServis.Domain.Responses.Cihaz;

public record CihazTanimResponse(
    Guid Id,
    Guid KategoriId,
    string KategoriAd,
    string Marka,
    string Model,
    bool Aktif
);