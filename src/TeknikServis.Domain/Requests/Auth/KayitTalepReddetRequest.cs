namespace TeknikServis.Domain.Requests.Auth;

public record KayitTalepReddetRequest(
    Guid TalepId,
    string RedNedeni
);