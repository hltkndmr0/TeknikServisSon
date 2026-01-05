namespace TeknikServis.Domain.Requests.Cihaz;

public record CreateCihazTanimRequest(
    Guid KategoriId,
    string Marka,
    string Model
);