namespace TeknikServis.Domain.Requests.Auth;

public record KayitTalepOnaylaRequest(
    Guid TalepId,
    string? Notlar
);