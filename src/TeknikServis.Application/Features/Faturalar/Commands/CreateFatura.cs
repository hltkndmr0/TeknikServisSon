using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Requests.Fatura;
using TeknikServis.Domain.Responses.Fatura;

namespace TeknikServis.Application.Features.Faturalar.Commands;

// Command
public record CreateFaturaCommand(CreateFaturaRequest Data) : IRequest<Result<FaturaResponse>>;

// Validator
public class CreateFaturaValidator : AbstractValidator<CreateFaturaCommand>
{
    public CreateFaturaValidator()
    {
        RuleFor(x => x.Data.IsEmriId)
            .NotEmpty().WithMessage("İş emri zorunludur");

        RuleFor(x => x.Data.Kalemler)
            .NotEmpty().WithMessage("En az bir kalem eklemelisiniz");

        RuleFor(x => x.Data.KdvOrani)
            .GreaterThanOrEqualTo(0).WithMessage("KDV oranı 0 veya daha büyük olmalıdır");
    }
}

// Handler
public class CreateFaturaHandler : IRequestHandler<CreateFaturaCommand, Result<FaturaResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateFaturaHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<FaturaResponse>> Handle(CreateFaturaCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        if (!_currentUserService.KullaniciId.HasValue)
            return Result<FaturaResponse>.Fail("Kullanıcı bilgisi bulunamadı");

        if (!_currentUserService.DukkanId.HasValue)
            return Result<FaturaResponse>.Fail("Dükkan bilgisi bulunamadı");

        var isEmri = await _context.IsEmirleri
            .FirstOrDefaultAsync(x => x.Id == request.IsEmriId, cancellationToken);

        if (isEmri == null)
            return Result<FaturaResponse>.Fail("İş emri bulunamadı");

        // Fatura numarası oluştur
        var yil = DateTime.UtcNow.Year;
        var sonFaturaNo = await _context.Faturalar
            .Where(x => x.IsEmri.DukkanId == _currentUserService.DukkanId.Value)
            .OrderByDescending(x => x.OlusturmaTarihi)
            .Select(x => x.FaturaNo)
            .FirstOrDefaultAsync(cancellationToken);

        var yeniNo = 1;
        if (!string.IsNullOrEmpty(sonFaturaNo))
        {
            var noParts = sonFaturaNo.Split('-');
            if (noParts.Length == 2 && int.TryParse(noParts[1], out var sonNo))
            {
                yeniNo = sonNo + 1;
            }
        }

        var faturaNo = $"FTR-{yeniNo:D6}";

        var fatura = new Fatura
        {
            Id = Guid.NewGuid(),
            FaturaNo = faturaNo,
            IsEmriId = request.IsEmriId,
            TeklifId = request.TeklifId,
            KdvOrani = request.KdvOrani,
            Aciklama = request.Aciklama,
            OlusturanKullaniciId = _currentUserService.KullaniciId.Value
        };

        // Kalemleri ekle
        decimal toplamTutar = 0;
        var kalemResponses = new List<FaturaKalemResponse>();

        foreach (var kalemRequest in request.Kalemler)
        {
            var toplamFiyat = kalemRequest.Miktar * kalemRequest.BirimFiyat;
            toplamTutar += toplamFiyat;

            var kalem = new FaturaKalem
            {
                Id = Guid.NewGuid(),
                FaturaId = fatura.Id,
                KalemTipi = kalemRequest.KalemTipi,
                Aciklama = kalemRequest.Aciklama,
                Miktar = kalemRequest.Miktar,
                BirimFiyat = kalemRequest.BirimFiyat,
                ToplamFiyat = toplamFiyat
            };

            _context.FaturaKalemleri.Add(kalem);

            kalemResponses.Add(new FaturaKalemResponse(
                kalem.Id,
                kalem.KalemTipi,
                kalem.Aciklama,
                kalem.Miktar,
                kalem.BirimFiyat,
                kalem.ToplamFiyat
            ));
        }

        fatura.ToplamTutar = toplamTutar;
        fatura.KdvTutar = toplamTutar * request.KdvOrani / 100;
        fatura.GenelToplam = toplamTutar + fatura.KdvTutar;

        _context.Faturalar.Add(fatura);
        await _context.SaveChangesAsync(cancellationToken);

        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(x => x.Id == _currentUserService.KullaniciId.Value, cancellationToken);

        var response = new FaturaResponse(
            fatura.Id,
            fatura.FaturaNo,
            fatura.IsEmriId,
            isEmri.IsEmriNo,
            fatura.TeklifId,
            fatura.FaturaTarihi,
            fatura.ToplamTutar,
            fatura.KdvOrani,
            fatura.KdvTutar,
            fatura.GenelToplam,
            fatura.Aciklama,
            fatura.OlusturanKullaniciId,
            kullanici?.AdSoyad ?? "",
            fatura.OlusturmaTarihi,
            kalemResponses
        );

        return Result<FaturaResponse>.Succeed(response, "Fatura başarıyla oluşturuldu");
    }
}