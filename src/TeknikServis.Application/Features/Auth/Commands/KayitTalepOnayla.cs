using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Enums;
using TeknikServis.Domain.Requests.Auth;
using TeknikServis.Domain.Responses.Auth;

namespace TeknikServis.Application.Features.Auth.Commands;

// Command
public record KayitTalepOnaylaCommand(KayitTalepOnaylaRequest Data) : IRequest<Result<KayitTalepResponse>>;

// Validator
public class KayitTalepOnaylaValidator : AbstractValidator<KayitTalepOnaylaCommand>
{
    public KayitTalepOnaylaValidator()
    {
        RuleFor(x => x.Data.TalepId)
            .NotEmpty().WithMessage("Talep ID zorunludur");
    }
}

// Handler
public class KayitTalepOnaylaHandler : IRequestHandler<KayitTalepOnaylaCommand, Result<KayitTalepResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFirmaKoduGenerator _firmaKoduGenerator;

    public KayitTalepOnaylaHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IFirmaKoduGenerator firmaKoduGenerator)
    {
        _context = context;
        _currentUserService = currentUserService;
        _firmaKoduGenerator = firmaKoduGenerator;
    }

    public async Task<Result<KayitTalepResponse>> Handle(KayitTalepOnaylaCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        var kayitTalep = await _context.KayitTalepleri
            .FirstOrDefaultAsync(x => x.Id == request.TalepId, cancellationToken);

        if (kayitTalep == null)
            return Result<KayitTalepResponse>.Fail("Kayıt talebi bulunamadı");

        if (kayitTalep.Durum != KayitTalepDurumu.Beklemede)
            return Result<KayitTalepResponse>.Fail("Bu talep zaten işlem görmüş");

        // Firma kodu ve şifre oluştur
        var firmaKodu = _firmaKoduGenerator.Generate();
        var tempSifre = Guid.NewGuid().ToString("N")[..8];

        // Dükkan oluştur
        var dukkan = new Dukkan
        {
            Id = Guid.NewGuid(),
            Ad = kayitTalep.FirmaAdi,
            Telefon = kayitTalep.FirmaTelefon,
            Email = kayitTalep.FirmaEmail,
            Adres = kayitTalep.FirmaAdres,
            VergiNo = kayitTalep.VergiNo,
            FirmaKodu = firmaKodu,
            Aktif = true,
            AbonelikBitisTarihi = DateTime.UtcNow.AddMonths(1)
        };

        // Yetkili kullanıcı oluştur
        var kullanici = new Kullanici
        {
            Id = Guid.NewGuid(),
            DukkanId = dukkan.Id,
            Email = kayitTalep.YetkiliEmail,
            SifreHash = BCrypt.Net.BCrypt.HashPassword(tempSifre),
            AdSoyad = kayitTalep.YetkiliAdSoyad,
            Telefon = kayitTalep.YetkiliTelefon,
            Rol = KullaniciRol.DukkanKullanici,
            Aktif = true
        };

        // Talebi güncelle - şifre ve firma kodunu kaydet
        kayitTalep.Durum = KayitTalepDurumu.Onaylandi;
        kayitTalep.IslemYapanKullaniciId = _currentUserService.KullaniciId;
        kayitTalep.IslemTarihi = DateTime.UtcNow;
        kayitTalep.Notlar = request.Notlar;
        kayitTalep.OlusturulanFirmaKodu = firmaKodu;
        kayitTalep.OlusturulanSifre = tempSifre;

        _context.Dukkanlar.Add(dukkan);
        _context.Kullanicilar.Add(kullanici);
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

        return Result<KayitTalepResponse>.Succeed(response, $"Kayıt onaylandı. Firma Kodu: {firmaKodu}, Geçici Şifre: {tempSifre}");
    }
}