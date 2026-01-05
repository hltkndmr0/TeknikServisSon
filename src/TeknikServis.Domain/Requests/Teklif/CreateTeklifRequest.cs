namespace TeknikServis.Domain.Requests.Teklif;

public record CreateTeklifRequest(
    Guid IsEmriId,
    string? Aciklama,
    DateTime? GecerlilikTarihi,
    List<TeklifKalemRequest> Kalemler
);