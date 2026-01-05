using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Requests.Teklif;

public record TeklifKalemRequest(
    KalemTipi KalemTipi,
    string Aciklama,
    int Miktar,
    decimal BirimFiyat
);