namespace TeknikServis.Domain.Requests.Cihaz;

public record GetCihazlarRequest(
    string? Arama = null,
    int Page = 1,
    int PageSize = 20
);