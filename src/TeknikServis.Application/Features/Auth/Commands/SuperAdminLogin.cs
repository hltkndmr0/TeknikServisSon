using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Enums;
using TeknikServis.Domain.Requests.Auth;
using TeknikServis.Domain.Responses.Auth;

namespace TeknikServis.Application.Features.Auth.Commands;

// Request


// Command
public record SuperAdminLoginCommand(SuperAdminLoginRequest Data) : IRequest<Result<LoginResponse>>;

// Validator
public class SuperAdminLoginValidator : AbstractValidator<SuperAdminLoginCommand>
{
    public SuperAdminLoginValidator()
    {
        RuleFor(x => x.Data.Email)
            .NotEmpty().WithMessage("Email zorunludur")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz");

        RuleFor(x => x.Data.Sifre)
            .NotEmpty().WithMessage("Şifre zorunludur");
    }
}

    public class SuperAdminLoginHandler : IRequestHandler<SuperAdminLoginCommand, Result<LoginResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public SuperAdminLoginHandler(IApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<Result<LoginResponse>> Handle(SuperAdminLoginCommand command, CancellationToken cancellationToken)
        {
            var request = command.Data;

            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(x => x.Email == request.Email && x.Rol == KullaniciRol.SuperAdmin && x.Aktif, cancellationToken);

            if (kullanici == null)
                return Result<LoginResponse>.Fail("Geçersiz email veya şifre");

            if (!BCrypt.Net.BCrypt.Verify(request.Sifre, kullanici.SifreHash))
                return Result<LoginResponse>.Fail("Geçersiz email veya şifre");

            var token = _jwtService.GenerateToken(kullanici, null);
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
                    null,
                    null
                )
            );

            return Result<LoginResponse>.Succeed(response, "Giriş başarılı");
        }
    }