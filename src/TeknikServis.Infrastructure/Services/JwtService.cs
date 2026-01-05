using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Domain.Entities;

namespace TeknikServis.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Kullanici kullanici, string? dukkanAdi)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
            new(ClaimTypes.Email, kullanici.Email),
            new(ClaimTypes.Name, kullanici.AdSoyad),
            new(ClaimTypes.Role, kullanici.Rol.ToString())
        };

        if (kullanici.DukkanId.HasValue)
        {
            claims.Add(new Claim("DukkanId", kullanici.DukkanId.Value.ToString()));
        }

        if (!string.IsNullOrEmpty(dukkanAdi))
        {
            claims.Add(new Claim("DukkanAdi", dukkanAdi));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}