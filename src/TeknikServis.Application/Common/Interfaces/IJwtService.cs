using TeknikServis.Domain.Entities;

namespace TeknikServis.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(Kullanici kullanici, string? dukkanAdi);
    string GenerateRefreshToken();
}