namespace TeknikServis.Domain.Requests.Auth;

public record KayitTalepOlusturRequest(
    string FirmaAdi,
    string FirmaTelefon,
    string FirmaEmail,
    string? FirmaAdres,
    string? VergiNo,
    string YetkiliAdSoyad,
    string YetkiliTelefon,
    string YetkiliEmail
);