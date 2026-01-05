using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TeknikServis.Application.Common.Interfaces;

namespace TeknikServis.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? KullaniciId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(id) ? null : Guid.Parse(id);
        }
    }

    public Guid? DukkanId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User?.FindFirstValue("DukkanId");
            return string.IsNullOrEmpty(id) ? null : Guid.Parse(id);
        }
    }

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public string? Rol => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}