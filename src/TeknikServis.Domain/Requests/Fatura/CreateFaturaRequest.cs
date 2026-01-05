namespace TeknikServis.Domain.Requests.Fatura;

public record CreateFaturaRequest(
    Guid IsEmriId,
    Guid? TeklifId,
    int KdvOrani,
    string? Aciklama,
    List<FaturaKalemRequest> Kalemler
);