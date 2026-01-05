using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Enums;
using TeknikServis.Domain.Requests.Auth;
using TeknikServis.Domain.Responses.Auth;

namespace TeknikServis.Application.Features.Auth.Commands;

// Command
public record KayitTalepReddetCommand(KayitTalepReddetRequest Data) : IRequest<Result<KayitTalepResponse>>;

// Validator
public class KayitTalepReddetValidator : AbstractValidator<KayitTalepReddetCommand>
{
    public KayitTalepReddetValidator()
    {
        RuleFor(x => x.Data.TalepId)
            .NotEmpty().WithMessage("Talep ID zorunludur");

        RuleFor(x => x.Data.RedNedeni)
            .NotEmpty().WithMessage("Red nedeni zorunludur");
    }
}

// Handler
public class KayitTalepReddetHandler : IRequestHandler<KayitTalepReddetCommand, Result<KayitTalepResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public KayitTalepReddetHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<KayitTalepResponse>> Handle(KayitTalepReddetCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        var kayitTalep = await _context.KayitTalepleri
            .FirstOrDefaultAsync(x => x.Id == request.TalepId, cancellationToken);

        if (kayitTalep == null)
            return Result<KayitTalepResponse>.Fail("Kayıt talebi bulunamadı");

        if (kayitTalep.Durum != KayitTalepDurumu.Beklemede)
            return Result<KayitTalepResponse>.Fail("Bu talep zaten işlem görmüş");

        kayitTalep.Durum = KayitTalepDurumu.Reddedildi;
        kayitTalep.RedNedeni = request.RedNedeni;
        kayitTalep.IslemYapanKullaniciId = _currentUserService.KullaniciId;
        kayitTalep.IslemTarihi = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var response = new KayitTalepResponse(
            kayitTalep.Id,
            kayitTalep.FirmaAdi,
            kayitTalep.FirmaTelefon,
            kayitTalep.FirmaEmail,
            kayitTalep.FirmaAdres,
            kayitTalep.VergiNo,
            kayitTalep.YetkiliAdSoyad,
            kayitTalep.YetkiliTelefon,
            kayitTalep.YetkiliEmail,
            kayitTalep.Durum,
            kayitTalep.RedNedeni,
            kayitTalep.TalepTarihi,
            kayitTalep.IslemTarihi,
            kayitTalep.OlusturulanSifre,
            kayitTalep.OlusturulanSifre
        );

        return Result<KayitTalepResponse>.Succeed(response, "Kayıt talebi reddedildi");
    }
}