using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Responses.Teklif;

public record TeklifKalemResponse(
    Guid Id,
    KalemTipi KalemTipi,
    string Aciklama,
    int Miktar,
    decimal BirimFiyat,
    decimal ToplamFiyat
);