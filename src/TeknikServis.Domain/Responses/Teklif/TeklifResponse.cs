using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Responses.Teklif;

public record TeklifResponse(
    Guid Id,
    string TeklifNo,
    Guid IsEmriId,
    string IsEmriNo,
    TeklifDurumu Durum,
    decimal ToplamTutar,
    string? Aciklama,
    DateTime? GecerlilikTarihi,
    DateTime? OnayTarihi,
    bool MailGonderildi,
    DateTime? MailGonderimTarihi,
    Guid OlusturanKullaniciId,
    string OlusturanKullaniciAd,
    DateTime OlusturmaTarihi,
    List<TeklifKalemResponse> Kalemler
);