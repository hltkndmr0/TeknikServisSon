using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Responses.Fatura;

public record FaturaKalemResponse(
    Guid Id,
    KalemTipi KalemTipi,
    string Aciklama,
    int Miktar,
    decimal BirimFiyat,
    decimal ToplamFiyat
);