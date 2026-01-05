using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Responses.IsEmri;

public record IsEmriDetailResponse(
    Guid Id,
    string IsEmriNo,
    Guid DukkanId,
    string DukkanAd,
    Guid MusteriId,
    string MusteriAd,
    string MusteriTelefon,
    string? MusteriEmail,
    Guid CihazId,
    string CihazBilgi,
    string? SeriNo,
    string? Imei,
    Guid DurumId,
    string DurumAd,
    string? DurumRenk,
    Guid TeslimAlanKullaniciId,
    string TeslimAlanKullaniciAd,
    Guid? AtananKullaniciId,
    string? AtananKullaniciAd,
    string? ArizaAciklamasi,
    string? OnTeshis,
    string? YapilanIslem,
    int? TahminiSureGun,
    decimal? TahminiUcret,
    decimal? KesinUcret,
    bool GarantiKapsaminda,
    Oncelik Oncelik,
    DateTime? TeslimTarihi,
    DateTime? TamamlanmaTarihi,
    DateTime OlusturmaTarihi,
    List<IsNotuResponse> Notlar,
    List<IsDurumGecmisiResponse> DurumGecmisi
);