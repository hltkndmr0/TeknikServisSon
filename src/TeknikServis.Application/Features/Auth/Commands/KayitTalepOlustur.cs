using FluentValidation;
using MediatR;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Requests.Auth;
using TeknikServis.Domain.Responses.Auth;

namespace TeknikServis.Application.Features.Auth.Commands;

// Command
public record KayitTalepOlusturCommand(KayitTalepOlusturRequest Data) : IRequest<Result<KayitTalepResponse>>;

// Validator
public class KayitTalepOlusturValidator : AbstractValidator<KayitTalepOlusturCommand>
{
    public KayitTalepOlusturValidator()
    {
        RuleFor(x => x.Data.FirmaAdi)
            .NotEmpty().WithMessage("Firma adı zorunludur")
            .MaximumLength(200).WithMessage("Firma adı en fazla 200 karakter olabilir");

        RuleFor(x => x.Data.FirmaTelefon)
            .NotEmpty().WithMessage("Firma telefonu zorunludur");

        RuleFor(x => x.Data.FirmaEmail)
            .NotEmpty().WithMessage("Firma email zorunludur")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz");

        RuleFor(x => x.Data.YetkiliAdSoyad)
            .NotEmpty().WithMessage("Yetkili ad soyad zorunludur");

        RuleFor(x => x.Data.YetkiliTelefon)
            .NotEmpty().WithMessage("Yetkili telefonu zorunludur");

        RuleFor(x => x.Data.YetkiliEmail)
            .NotEmpty().WithMessage("Yetkili email zorunludur")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz");
    }
}

// Handler
public class KayitTalepOlusturHandler : IRequestHandler<KayitTalepOlusturCommand, Result<KayitTalepResponse>>
{
    private readonly IApplicationDbContext _context;

    public KayitTalepOlusturHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<KayitTalepResponse>> Handle(KayitTalepOlusturCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        var kayitTalep = new KayitTalep
        {
            Id = Guid.NewGuid(),
            FirmaAdi = request.FirmaAdi,
            FirmaTelefon = request.FirmaTelefon,
            FirmaEmail = request.FirmaEmail,
            FirmaAdres = request.FirmaAdres,
            VergiNo = request.VergiNo,
            YetkiliAdSoyad = request.YetkiliAdSoyad,
            YetkiliTelefon = request.YetkiliTelefon,
            YetkiliEmail = request.YetkiliEmail
        };

        _context.KayitTalepleri.Add(kayitTalep);
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
            kayitTalep.OlusturulanFirmaKodu,
            kayitTalep.OlusturulanSifre
        );

        return Result<KayitTalepResponse>.Succeed(response, "Kayıt talebiniz alınmıştır. En kısa sürede sizinle iletişime geçilecektir.");
    }
}