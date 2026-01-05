using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Requests.Teklif;
using TeknikServis.Domain.Responses.Teklif;

namespace TeknikServis.Application.Features.Teklifler.Commands;

// Command
public record CreateTeklifCommand(CreateTeklifRequest Data) : IRequest<Result<TeklifResponse>>;

// Validator
public class CreateTeklifValidator : AbstractValidator<CreateTeklifCommand>
{
    public CreateTeklifValidator()
    {
        RuleFor(x => x.Data.IsEmriId)
            .NotEmpty().WithMessage("İş emri zorunludur");

        RuleFor(x => x.Data.Kalemler)
            .NotEmpty().WithMessage("En az bir kalem eklemelisiniz");
    }
}

// Handler
public class CreateTeklifHandler : IRequestHandler<CreateTeklifCommand, Result<TeklifResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateTeklifHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<TeklifResponse>> Handle(CreateTeklifCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        if (!_currentUserService.KullaniciId.HasValue)
            return Result<TeklifResponse>.Fail("Kullanıcı bilgisi bulunamadı");

        var isEmri = await _context.IsEmirleri
            .FirstOrDefaultAsync(x => x.Id == request.IsEmriId, cancellationToken);

        if (isEmri == null)
            return Result<TeklifResponse>.Fail("İş emri bulunamadı");

        // Teklif numarası oluştur
        var sonTeklifNo = await _context.Teklifler
            .Where(x => x.IsEmriId == request.IsEmriId)
            .OrderByDescending(x => x.OlusturmaTarihi)
            .Select(x => x.TeklifNo)
            .FirstOrDefaultAsync(cancellationToken);

        var yeniNo = 1;
        if (!string.IsNullOrEmpty(sonTeklifNo))
        {
            var noParts = sonTeklifNo.Split('-');
            if (noParts.Length == 2 && int.TryParse(noParts[1], out var sonNo))
            {
                yeniNo = sonNo + 1;
            }
        }

        var teklifNo = $"TKL-{yeniNo:D3}";

        var teklif = new Teklif
        {
            Id = Guid.NewGuid(),
            TeklifNo = teklifNo,
            IsEmriId = request.IsEmriId,
            Aciklama = request.Aciklama,
            GecerlilikTarihi = request.GecerlilikTarihi,
            OlusturanKullaniciId = _currentUserService.KullaniciId.Value
        };

        // Kalemleri ekle
        decimal toplamTutar = 0;
        var kalemResponses = new List<TeklifKalemResponse>();

        foreach (var kalemRequest in request.Kalemler)
        {
            var toplamFiyat = kalemRequest.Miktar * kalemRequest.BirimFiyat;
            toplamTutar += toplamFiyat;

            var kalem = new TeklifKalem
            {
                Id = Guid.NewGuid(),
                TeklifId = teklif.Id,
                KalemTipi = kalemRequest.KalemTipi,
                Aciklama = kalemRequest.Aciklama,
                Miktar = kalemRequest.Miktar,
                BirimFiyat = kalemRequest.BirimFiyat,
                ToplamFiyat = toplamFiyat
            };

            _context.TeklifKalemleri.Add(kalem);

            kalemResponses.Add(new TeklifKalemResponse(
                kalem.Id,
                kalem.KalemTipi,
                kalem.Aciklama,
                kalem.Miktar,
                kalem.BirimFiyat,
                kalem.ToplamFiyat
            ));
        }

        teklif.ToplamTutar = toplamTutar;

        _context.Teklifler.Add(teklif);
        await _context.SaveChangesAsync(cancellationToken);

        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(x => x.Id == _currentUserService.KullaniciId.Value, cancellationToken);

        var response = new TeklifResponse(
            teklif.Id,
            teklif.TeklifNo,
            teklif.IsEmriId,
            isEmri.IsEmriNo,
            teklif.Durum,
            teklif.ToplamTutar,
            teklif.Aciklama,
            teklif.GecerlilikTarihi,
            teklif.OnayTarihi,
            teklif.MailGonderildi,
            teklif.MailGonderimTarihi,
            teklif.OlusturanKullaniciId,
            kullanici?.AdSoyad ?? "",
            teklif.OlusturmaTarihi,
            kalemResponses
        );

        return Result<TeklifResponse>.Succeed(response, "Teklif başarıyla oluşturuldu");
    }
}