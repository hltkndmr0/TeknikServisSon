using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Requests.IsEmri;

public record CreateIsEmriRequest(
    Guid MusteriId,
    Guid CihazId,
    string? ArizaAciklamasi,
    string? OnTeshis,
    int? TahminiSureGun,
    decimal? TahminiUcret,
    bool GarantiKapsaminda,
    Oncelik Oncelik
);