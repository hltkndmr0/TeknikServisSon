using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Requests.IsEmri;
using TeknikServis.Domain.Responses.IsEmri;

namespace TeknikServis.Application.Features.IsEmirleri.Commands;

// Command
public record UpdateIsEmriDurumCommand(UpdateIsEmriDurumRequest Data) : IRequest<Result<IsEmriResponse>>;

// Validator
public class UpdateIsEmriDurumValidator : AbstractValidator<UpdateIsEmriDurumCommand>
{
    public UpdateIsEmriDurumValidator()
    {
        RuleFor(x => x.Data.IsEmriId)
            .NotEmpty().WithMessage("İş emri ID zorunludur");

        RuleFor(x => x.Data.DurumId)
            .NotEmpty().WithMessage("Durum ID zorunludur");
    }
}

// Handler
public class UpdateIsEmriDurumHandler : IRequestHandler<UpdateIsEmriDurumCommand, Result<IsEmriResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateIsEmriDurumHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<IsEmriResponse>> Handle(UpdateIsEmriDurumCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        if (!_currentUserService.KullaniciId.HasValue)
            return Result<IsEmriResponse>.Fail("Kullanıcı bilgisi bulunamadı");

        var isEmri = await _context.IsEmirleri
            .Include(x => x.Musteri)
            .Include(x => x.Cihaz)
            .ThenInclude(x => x.CihazTanim)
            .FirstOrDefaultAsync(x => x.Id == request.IsEmriId, cancellationToken);

        if (isEmri == null)
            return Result<IsEmriResponse>.Fail("İş emri bulunamadı");

        var yeniDurum = await _context.IsDurumlari
            .FirstOrDefaultAsync(x => x.Id == request.DurumId, cancellationToken);

        if (yeniDurum == null)
            return Result<IsEmriResponse>.Fail("Durum bulunamadı");

        // Durumu güncelle
        isEmri.DurumId = request.DurumId;

        // Tamamlandı durumuna geçtiyse tarihi kaydet
        if (yeniDurum.Ad == "Tamamlandı")
            isEmri.TamamlanmaTarihi = DateTime.UtcNow;

        // Teslim edildi durumuna geçtiyse tarihi kaydet
        if (yeniDurum.Ad == "Teslim Edildi")
            isEmri.TeslimTarihi = DateTime.UtcNow;

        // Durum geçmişi ekle
        var durumGecmisi = new IsDurumGecmisi
        {
            Id = Guid.NewGuid(),
            IsEmriId = isEmri.Id,
            DurumId = request.DurumId,
            KullaniciId = _currentUserService.KullaniciId.Value,
            Aciklama = request.Aciklama
        };

        _context.IsDurumGecmisleri.Add(durumGecmisi);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new IsEmriResponse(
            isEmri.Id,
            isEmri.IsEmriNo,
            isEmri.DukkanId,
            isEmri.MusteriId,
            isEmri.Musteri.AdSoyad ?? isEmri.Musteri.FirmaAdi ?? "",
            isEmri.Musteri.Telefon,
            isEmri.CihazId,
            $"{isEmri.Cihaz.CihazTanim.Marka} {isEmri.Cihaz.CihazTanim.Model}",
            isEmri.Cihaz.SeriNo,
            isEmri.DurumId,
            yeniDurum.Ad,
            yeniDurum.Renk,
            isEmri.ArizaAciklamasi,
            isEmri.Oncelik,
            isEmri.TahminiUcret,
            isEmri.OlusturmaTarihi
        );

        return Result<IsEmriResponse>.Succeed(response, "İş emri durumu güncellendi");
    }
}