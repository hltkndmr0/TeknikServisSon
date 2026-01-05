namespace TeknikServis.Domain.Responses.IsEmri;

public record IsNotuResponse(
    Guid Id,
    Guid KullaniciId,
    string KullaniciAd,
    string NotMetni,
    DateTime OlusturmaTarihi
);