namespace TeknikServis.Domain.Responses.Auth;

public record LoginResponse(
    string Token,
    string RefreshToken,
    DateTime TokenExpiry,
    KullaniciResponse Kullanici
);

public record KullaniciResponse(
    Guid Id,
    string Email,
    string AdSoyad,
    string Rol,
    Guid? DukkanId,
    string? DukkanAdi
);