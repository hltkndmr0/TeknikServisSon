using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Responses.IsEmri;

public record IsEmriResponse(
    Guid Id,
    string IsEmriNo,
    Guid DukkanId,
    Guid MusteriId,
    string MusteriAd,
    string MusteriTelefon,
    Guid CihazId,
    string CihazBilgi,
    string? SeriNo,
    Guid DurumId,
    string DurumAd,
    string? DurumRenk,
    string? ArizaAciklamasi,
    Oncelik Oncelik,
    decimal? TahminiUcret,
    DateTime OlusturmaTarihi
);