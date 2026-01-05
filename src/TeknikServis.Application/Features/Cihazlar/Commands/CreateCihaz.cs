using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Requests.Cihaz;
using TeknikServis.Domain.Responses.Cihaz;

namespace TeknikServis.Application.Features.Cihazlar.Commands;

// Command
public record CreateCihazCommand(CreateCihazRequest Data) : IRequest<Result<CihazResponse>>;

// Validator
public class CreateCihazValidator : AbstractValidator<CreateCihazCommand>
{
    public CreateCihazValidator()
    {
        RuleFor(x => x.Data.CihazTanimId)
            .NotEmpty().WithMessage("Cihaz tanımı zorunludur");

        RuleFor(x => x.Data.MusteriId)
            .NotEmpty().WithMessage("Müşteri zorunludur");
    }
}

// Handler
public class CreateCihazHandler : IRequestHandler<CreateCihazCommand, Result<CihazResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateCihazHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CihazResponse>> Handle(CreateCihazCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        // Seri no kontrolü
        if (!string.IsNullOrEmpty(request.SeriNo))
        {
            var seriNoVar = await _context.Cihazlar
                .AnyAsync(x => x.SeriNo == request.SeriNo, cancellationToken);

            if (seriNoVar)
                return Result<CihazResponse>.Fail("Bu seri numarası zaten kayıtlı");
        }

        // IMEI kontrolü
        if (!string.IsNullOrEmpty(request.Imei))
        {
            var imeiVar = await _context.Cihazlar
                .AnyAsync(x => x.Imei == request.Imei, cancellationToken);

            if (imeiVar)
                return Result<CihazResponse>.Fail("Bu IMEI zaten kayıtlı");
        }

        var cihazTanim = await _context.CihazTanimlari
            .Include(x => x.Kategori)
            .FirstOrDefaultAsync(x => x.Id == request.CihazTanimId, cancellationToken);

        if (cihazTanim == null)
            return Result<CihazResponse>.Fail("Cihaz tanımı bulunamadı");

        var musteri = await _context.Musteriler
            .FirstOrDefaultAsync(x => x.Id == request.MusteriId, cancellationToken);

        if (musteri == null)
            return Result<CihazResponse>.Fail("Müşteri bulunamadı");

        var cihaz = new Cihaz
        {
            Id = Guid.NewGuid(),
            CihazTanimId = request.CihazTanimId,
            MusteriId = request.MusteriId,
            SeriNo = request.SeriNo,
            Imei = request.Imei,
            Renk = request.Renk,
            GarantiBitisTarihi = request.GarantiBitisTarihi,
            Notlar = request.Notlar
        };

        _context.Cihazlar.Add(cihaz);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new CihazResponse(
            cihaz.Id,
            cihaz.CihazTanimId,
            cihazTanim.Marka,
            cihazTanim.Model,
            cihazTanim.Kategori.Ad,
            cihaz.MusteriId,
            musteri.AdSoyad ?? musteri.FirmaAdi,
            cihaz.SeriNo,
            cihaz.Imei,
            cihaz.Renk,
            cihaz.GarantiBitisTarihi,
            cihaz.Notlar,
            cihaz.OlusturmaTarihi
        );

        return Result<CihazResponse>.Succeed(response, "Cihaz başarıyla oluşturuldu");
    }
}