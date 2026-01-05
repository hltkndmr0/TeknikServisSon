using FluentValidation;
using MediatR;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Enums;
using TeknikServis.Domain.Requests.Musteri;
using TeknikServis.Domain.Responses.Musteri;

namespace TeknikServis.Application.Features.Musteriler.Commands;

// Command
public record CreateMusteriCommand(CreateMusteriRequest Data) : IRequest<Result<MusteriResponse>>;

// Validator
public class CreateMusteriValidator : AbstractValidator<CreateMusteriCommand>
{
    public CreateMusteriValidator()
    {
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
public class CreateMusteriHandler : IRequestHandler<CreateMusteriCommand, Result<MusteriResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateMusteriHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<MusteriResponse>> Handle(CreateMusteriCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        var musteri = new Musteri
        {
            Id = Guid.NewGuid(),
            MusteriTipi = request.MusteriTipi,
            AdSoyad = request.AdSoyad,
            FirmaAdi = request.FirmaAdi,
            VergiNo = request.VergiNo,
            Telefon = request.Telefon,
            Telefon2 = request.Telefon2,
            Email = request.Email,
            Adres = request.Adres,
            Notlar = request.Notlar
        };

        _context.Musteriler.Add(musteri);

        // Dükkan-Müşteri ilişkisi
        if (_currentUserService.DukkanId.HasValue)
        {
            var dukkanMusteri = new DukkanMusteri
            {
                Id = Guid.NewGuid(),
                DukkanId = _currentUserService.DukkanId.Value,
                MusteriId = musteri.Id
            };
            _context.DukkanMusterileri.Add(dukkanMusteri);
        }

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

        return Result<MusteriResponse>.Succeed(response, "Müşteri başarıyla oluşturuldu");
    }
}