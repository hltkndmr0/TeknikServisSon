using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Requests.IsEmri;

public record UpdateIsEmriRequest(
    Guid Id,
    string? ArizaAciklamasi,
    string? OnTeshis,
    string? YapilanIslem,
    int? TahminiSureGun,
    decimal? TahminiUcret,
    decimal? KesinUcret,
    bool GarantiKapsaminda,
    Oncelik Oncelik,
    Guid? AtananKullaniciId
);