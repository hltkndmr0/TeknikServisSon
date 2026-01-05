namespace TeknikServis.Domain.Requests.Auth;

public record LoginRequest(
    string FirmaKodu,
    string Email,
    string Sifre
);