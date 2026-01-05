using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Requests.Cihaz;
using TeknikServis.Domain.Responses.Cihaz;

namespace TeknikServis.Application.Features.Cihazlar.Commands;

// Command
public record UpdateCihazCommand(UpdateCihazRequest Data) : IRequest<Result<CihazResponse>>;

// Validator
public class UpdateCihazValidator : AbstractValidator<UpdateCihazCommand>
{
    public UpdateCihazValidator()
    {
        RuleFor(x => x.Data.Id)
            .NotEmpty().WithMessage("Cihaz ID zorunludur");
    }
}

// Handler
public class UpdateCihazHandler : IRequestHandler<UpdateCihazCommand, Result<CihazResponse>>
{
    private readonly IApplicationDbContext _context;

    public UpdateCihazHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CihazResponse>> Handle(UpdateCihazCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        var cihaz = await _context.Cihazlar
            .Include(x => x.CihazTanim)
            .ThenInclude(x => x.Kategori)
            .Include(x => x.Musteri)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (cihaz == null)
            return Result<CihazResponse>.Fail("Cihaz bulunamadı");

        // Seri no kontrolü
        if (!string.IsNullOrEmpty(request.SeriNo) && request.SeriNo != cihaz.SeriNo)
        {
            var seriNoVar = await _context.Cihazlar
                .AnyAsync(x => x.SeriNo == request.SeriNo && x.Id != request.Id, cancellationToken);

            if (seriNoVar)
                return Result<CihazResponse>.Fail("Bu seri numarası zaten kayıtlı");
        }

        // IMEI kontrolü
        if (!string.IsNullOrEmpty(request.Imei) && request.Imei != cihaz.Imei)
        {
            var imeiVar = await _context.Cihazlar
                .AnyAsync(x => x.Imei == request.Imei && x.Id != request.Id, cancellationToken);

            if (imeiVar)
                return Result<CihazResponse>.Fail("Bu IMEI zaten kayıtlı");
        }

        cihaz.SeriNo = request.SeriNo;
        cihaz.Imei = request.Imei;
        cihaz.Renk = request.Renk;
        cihaz.GarantiBitisTarihi = request.GarantiBitisTarihi;
        cihaz.Notlar = request.Notlar;

        await _context.SaveChangesAsync(cancellationToken);

        var response = new CihazResponse(
            cihaz.Id,
            cihaz.CihazTanimId,
            cihaz.CihazTanim.Marka,
            cihaz.CihazTanim.Model,
            cihaz.CihazTanim.Kategori.Ad,
            cihaz.MusteriId,
            cihaz.Musteri.AdSoyad ?? cihaz.Musteri.FirmaAdi,
            cihaz.SeriNo,
            cihaz.Imei,
            cihaz.Renk,
            cihaz.GarantiBitisTarihi,
            cihaz.Notlar,
            cihaz.OlusturmaTarihi
        );

        return Result<CihazResponse>.Succeed(response, "Cihaz başarıyla güncellendi");
    }
}