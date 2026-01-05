namespace TeknikServis.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? KullaniciId { get; }
    Guid? DukkanId { get; }
    string? Email { get; }
    string? Rol { get; }
    bool IsAuthenticated { get; }
}