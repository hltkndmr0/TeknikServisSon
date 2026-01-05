namespace TeknikServis.Domain.Responses.IsEmri;

public record IsDurumGecmisiResponse(
    Guid Id,
    string DurumAd,
    string? DurumRenk,
    string KullaniciAd,
    string? Aciklama,
    DateTime DegisimTarihi
);