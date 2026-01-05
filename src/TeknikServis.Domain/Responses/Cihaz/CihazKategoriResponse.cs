namespace TeknikServis.Domain.Responses.Cihaz;

public record CihazKategoriResponse(
    Guid Id,
    string Ad,
    bool Aktif
);