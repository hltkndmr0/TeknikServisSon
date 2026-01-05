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
public record CreateIsEmriCommand(CreateIsEmriRequest Data) : IRequest<Result<IsEmriResponse>>;

// Validator
public class CreateIsEmriValidator : AbstractValidator<CreateIsEmriCommand>
{
    public CreateIsEmriValidator()
    {
        RuleFor(x => x.Data.MusteriId)
            .NotEmpty().WithMessage("Müşteri zorunludur");

        RuleFor(x => x.Data.CihazId)
            .NotEmpty().WithMessage("Cihaz zorunludur");
    }
}

// Handler
public class CreateIsEmriHandler : IRequestHandler<CreateIsEmriCommand, Result<IsEmriResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateIsEmriHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<IsEmriResponse>> Handle(CreateIsEmriCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        if (!_currentUserService.DukkanId.HasValue)
            return Result<IsEmriResponse>.Fail("Dükkan bilgisi bulunamadı");

        if (!_currentUserService.KullaniciId.HasValue)
            return Result<IsEmriResponse>.Fail("Kullanıcı bilgisi bulunamadı");

        var dukkanId = _currentUserService.DukkanId.Value;
        var kullaniciId = _currentUserService.KullaniciId.Value;

        // Müşteri kontrolü
        var musteri = await _context.Musteriler
            .FirstOrDefaultAsync(x => x.Id == request.MusteriId, cancellationToken);

        if (musteri == null)
            return Result<IsEmriResponse>.Fail("Müşteri bulunamadı");

        // Cihaz kontrolü
        var cihaz = await _context.Cihazlar
            .Include(x => x.CihazTanim)
            .ThenInclude(x => x.Kategori)
            .FirstOrDefaultAsync(x => x.Id == request.CihazId, cancellationToken);

        if (cihaz == null)
            return Result<IsEmriResponse>.Fail("Cihaz bulunamadı");

        // İlk durum: Kabul Edildi
        var ilkDurum = await _context.IsDurumlari
            .FirstOrDefaultAsync(x => x.Sira == 1, cancellationToken);

        if (ilkDurum == null)
            return Result<IsEmriResponse>.Fail("İş durumu bulunamadı");

        // İş emri numarası oluştur
        var sonIsEmriNo = await _context.IsEmirleri
            .Where(x => x.DukkanId == dukkanId)
            .OrderByDescending(x => x.OlusturmaTarihi)
            .Select(x => x.IsEmriNo)
            .FirstOrDefaultAsync(cancellationToken);

        var yeniNo = 1;
        if (!string.IsNullOrEmpty(sonIsEmriNo))
        {
            var noParts = sonIsEmriNo.Split('-');
            if (noParts.Length == 2 && int.TryParse(noParts[1], out var sonNo))
            {
                yeniNo = sonNo + 1;
            }
        }

        var isEmriNo = $"IS-{yeniNo:D6}";

        var isEmri = new IsEmri
        {
            Id = Guid.NewGuid(),
            IsEmriNo = isEmriNo,
            DukkanId = dukkanId,
            MusteriId = request.MusteriId,
            CihazId = request.CihazId,
            DurumId = ilkDurum.Id,
            TeslimAlanKullaniciId = kullaniciId,
            ArizaAciklamasi = request.ArizaAciklamasi,
            OnTeshis = request.OnTeshis,
            TahminiSureGun = request.TahminiSureGun,
            TahminiUcret = request.TahminiUcret,
            GarantiKapsaminda = request.GarantiKapsaminda,
            Oncelik = request.Oncelik
        };

        // Durum geçmişi ekle
        var durumGecmisi = new IsDurumGecmisi
        {
            Id = Guid.NewGuid(),
            IsEmriId = isEmri.Id,
            DurumId = ilkDurum.Id,
            KullaniciId = kullaniciId,
            Aciklama = "İş emri oluşturuldu"
        };

        _context.IsEmirleri.Add(isEmri);
        _context.IsDurumGecmisleri.Add(durumGecmisi);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new IsEmriResponse(
            isEmri.Id,
            isEmri.IsEmriNo,
            isEmri.DukkanId,
            isEmri.MusteriId,
            musteri.AdSoyad ?? musteri.FirmaAdi ?? "",
            musteri.Telefon,
            isEmri.CihazId,
            $"{cihaz.CihazTanim.Marka} {cihaz.CihazTanim.Model}",
            cihaz.SeriNo,
            isEmri.DurumId,
            ilkDurum.Ad,
            ilkDurum.Renk,
            isEmri.ArizaAciklamasi,
            isEmri.Oncelik,
            isEmri.TahminiUcret,
            isEmri.OlusturmaTarihi
        );

        return Result<IsEmriResponse>.Succeed(response, "İş emri başarıyla oluşturuldu");
    }
}