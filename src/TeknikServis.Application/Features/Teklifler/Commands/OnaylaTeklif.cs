using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Enums;
using TeknikServis.Domain.Requests.Teklif;
using TeknikServis.Domain.Responses.Teklif;

namespace TeknikServis.Application.Features.Teklifler.Commands;

// Command
public record OnaylaTeklifCommand(OnaylaTeklifRequest Data) : IRequest<Result<TeklifResponse>>;

// Validator
public class OnaylaTeklifValidator : AbstractValidator<OnaylaTeklifCommand>
{
    public OnaylaTeklifValidator()
    {
        RuleFor(x => x.Data.TeklifId)
            .NotEmpty().WithMessage("Teklif ID zorunludur");
    }
}

// Handler
public class OnaylaTeklifHandler : IRequestHandler<OnaylaTeklifCommand, Result<TeklifResponse>>
{
    private readonly IApplicationDbContext _context;

    public OnaylaTeklifHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TeklifResponse>> Handle(OnaylaTeklifCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        var teklif = await _context.Teklifler
            .Include(x => x.IsEmri)
            .Include(x => x.OlusturanKullanici)
            .Include(x => x.Kalemler)
            .FirstOrDefaultAsync(x => x.Id == request.TeklifId, cancellationToken);

        if (teklif == null)
            return Result<TeklifResponse>.Fail("Teklif bulunamadı");

        if (teklif.Durum != TeklifDurumu.Bekliyor)
            return Result<TeklifResponse>.Fail("Bu teklif zaten işlem görmüş");

        teklif.Durum = TeklifDurumu.Onaylandi;
        teklif.OnayTarihi = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var kalemResponses = teklif.Kalemler
            .Select(x => new TeklifKalemResponse(
                x.Id,
                x.KalemTipi,
                x.Aciklama,
                x.Miktar,
                x.BirimFiyat,
                x.ToplamFiyat
            ))
            .ToList();

        var response = new TeklifResponse(
            teklif.Id,
            teklif.TeklifNo,
            teklif.IsEmriId,
            teklif.IsEmri.IsEmriNo,
            teklif.Durum,
            teklif.ToplamTutar,
            teklif.Aciklama,
            teklif.GecerlilikTarihi,
            teklif.OnayTarihi,
            teklif.MailGonderildi,
            teklif.MailGonderimTarihi,
            teklif.OlusturanKullaniciId,
            teklif.OlusturanKullanici.AdSoyad,
            teklif.OlusturmaTarihi,
            kalemResponses
        );

        return Result<TeklifResponse>.Succeed(response, "Teklif onaylandı");
    }
}