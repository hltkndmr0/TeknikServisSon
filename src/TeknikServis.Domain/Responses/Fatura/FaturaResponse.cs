namespace TeknikServis.Domain.Responses.Fatura;

public record FaturaResponse(
    Guid Id,
    string FaturaNo,
    Guid IsEmriId,
    string IsEmriNo,
    Guid? TeklifId,
    DateTime FaturaTarihi,
    decimal ToplamTutar,
    int KdvOrani,
    decimal KdvTutar,
    decimal GenelToplam,
    string? Aciklama,
    Guid OlusturanKullaniciId,
    string OlusturanKullaniciAd,
    DateTime OlusturmaTarihi,
    List<FaturaKalemResponse> Kalemler
);