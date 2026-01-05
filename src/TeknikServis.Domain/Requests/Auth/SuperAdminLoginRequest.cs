namespace TeknikServis.Domain.Requests.Auth;

public record SuperAdminLoginRequest(
    string Email,
    string Sifre
);