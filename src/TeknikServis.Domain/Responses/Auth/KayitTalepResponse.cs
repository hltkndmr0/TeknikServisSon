using TeknikServis.Domain.Enums;

namespace TeknikServis.Domain.Responses.Auth;

public record KayitTalepResponse(
    Guid Id,
    string FirmaAdi,
    string FirmaTelefon,
    string FirmaEmail,
    string? FirmaAdres,
    string? VergiNo,
    string YetkiliAdSoyad,
    string YetkiliTelefon,
    string YetkiliEmail,
    KayitTalepDurumu Durum,
    string? RedNedeni,
    DateTime TalepTarihi,
    DateTime? IslemTarihiiDateTime,
    string? OlusturulanFirmaKodu,
    string? OlusturulanSifre
);