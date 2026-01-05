using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Enums;
using TeknikServis.Domain.Requests.Musteri;
using TeknikServis.Domain.Responses.Musteri;

namespace TeknikServis.Application.Features.Musteriler.Commands;

// Command
public record UpdateMusteriCommand(UpdateMusteriRequest Data) : IRequest<Result<MusteriResponse>>;

// Validator
public class UpdateMusteriValidator : AbstractValidator<UpdateMusteriCommand>
{
    public UpdateMusteriValidator()
    {
        RuleFor(x => x.Data.Id)
            .NotEmpty().WithMessage("Müşteri ID zorunludur");

        RuleFor(x => x.Data.Telefon)
            .NotEmpty().WithMessage("Telefon zorunludur");

        RuleFor(x => x.Data.AdSoyad)
            .NotEmpty().When(x => x.Data.MusteriTipi == MusteriTipi.Bireysel)
            .WithMessage("Bireysel müşteri için ad soyad zorunludur");

        RuleFor(x => x.Data.FirmaAdi)
            .NotEmpty().When(x => x.Data.MusteriTipi == MusteriTipi.Kurumsal)
            .WithMessage("Kurumsal müşteri için firma adı zorunludur");
    }
}

// Handler
public class UpdateMusteriHandler : IRequestHandler<UpdateMusteriCommand, Result<MusteriResponse>>
{
    private readonly IApplicationDbContext _context;

    public UpdateMusteriHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<MusteriResponse>> Handle(UpdateMusteriCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        var musteri = await _context.Musteriler
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (musteri == null)
            return Result<MusteriResponse>.Fail("Müşteri bulunamadı");

        musteri.MusteriTipi = request.MusteriTipi;
        musteri.AdSoyad = request.AdSoyad;
        musteri.FirmaAdi = request.FirmaAdi;
        musteri.VergiNo = request.VergiNo;
        musteri.Telefon = request.Telefon;
        musteri.Telefon2 = request.Telefon2;
        musteri.Email = request.Email;
        musteri.Adres = request.Adres;
        musteri.Notlar = request.Notlar;

        await _context.SaveChangesAsync(cancellationToken);

        var response = new MusteriResponse(
            musteri.Id,
            musteri.MusteriTipi,
            musteri.AdSoyad,
            musteri.FirmaAdi,
            musteri.VergiNo,
            musteri.Telefon,
            musteri.Telefon2,
            musteri.Email,
            musteri.Adres,
            musteri.Notlar,
            musteri.OlusturmaTarihi
        );

        return Result<MusteriResponse>.Succeed(response, "Müşteri başarıyla güncellendi");
    }
}