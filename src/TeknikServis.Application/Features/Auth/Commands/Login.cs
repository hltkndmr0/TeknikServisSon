using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Requests.Auth;
using TeknikServis.Domain.Responses.Auth;

namespace TeknikServis.Application.Features.Auth.Commands;

// Command
public record LoginCommand(LoginRequest Data) : IRequest<Result<LoginResponse>>;

// Validator
public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Data.FirmaKodu)
            .NotEmpty().WithMessage("Firma kodu zorunludur");

        RuleFor(x => x.Data.Email)
            .NotEmpty().WithMessage("Email zorunludur")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz");

        RuleFor(x => x.Data.Sifre)
            .NotEmpty().WithMessage("Şifre zorunludur");
    }
}

// Handler
public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public LoginHandler(IApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        // Firma koduna göre dükkan bul
        var dukkan = await _context.Dukkanlar
            .FirstOrDefaultAsync(x => x.FirmaKodu == request.FirmaKodu && x.Aktif, cancellationToken);

        if (dukkan == null)
            return Result<LoginResponse>.Fail("Geçersiz firma kodu");

        // Abonelik kontrolü
        if (dukkan.AbonelikBitisTarihi.HasValue && dukkan.AbonelikBitisTarihi.Value < DateTime.UtcNow)
            return Result<LoginResponse>.Fail("Abonelik süresi dolmuş");

        // Kullanıcıyı bul
        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(x => x.DukkanId == dukkan.Id && x.Email == request.Email && x.Aktif, cancellationToken);

        if (kullanici == null)
            return Result<LoginResponse>.Fail("Geçersiz email veya şifre");

        // Şifre kontrolü
        if (!BCrypt.Net.BCrypt.Verify(request.Sifre, kullanici.SifreHash))
            return Result<LoginResponse>.Fail("Geçersiz email veya şifre");

        // Token oluştur
        var token = _jwtService.GenerateToken(kullanici, dukkan.Ad);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var response = new LoginResponse(
            token,
            refreshToken,
            DateTime.UtcNow.AddHours(24),
            new KullaniciResponse(
                kullanici.Id,
                kullanici.Email,
                kullanici.AdSoyad,
                kullanici.Rol.ToString(),
                kullanici.DukkanId,
                dukkan.Ad
            )
        );

        return Result<LoginResponse>.Succeed(response, "Giriş başarılı");
    }
}