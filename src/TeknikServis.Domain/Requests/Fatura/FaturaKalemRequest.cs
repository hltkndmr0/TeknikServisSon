using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Requests.Fatura;

public record FaturaKalemRequest(
    KalemTipi KalemTipi,
    string Aciklama,
    int Miktar,
    decimal BirimFiyat
);