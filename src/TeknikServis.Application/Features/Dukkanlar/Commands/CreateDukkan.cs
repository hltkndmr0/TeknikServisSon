using FluentValidation;
using MediatR;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Requests.Dukkan;
using TeknikServis.Domain.Responses.Dukkan;

namespace TeknikServis.Application.Features.Dukkanlar.Commands;

// Command
public record CreateDukkanCommand(CreateDukkanRequest Data) : IRequest<Result<DukkanResponse>>;

// Validator
public class CreateDukkanValidator : AbstractValidator<CreateDukkanCommand>
{
    public CreateDukkanValidator()
    {
        RuleFor(x => x.Data.Ad)
            .NotEmpty().WithMessage("Dükkan adı zorunludur")
            .MaximumLength(200).WithMessage("Dükkan adı en fazla 200 karakter olabilir");

        RuleFor(x => x.Data.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Data.Email))
            .WithMessage("Geçerli bir email adresi giriniz");
    }
}

// Handler
public class CreateDukkanHandler : IRequestHandler<CreateDukkanCommand, Result<DukkanResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFirmaKoduGenerator _firmaKoduGenerator;

    public CreateDukkanHandler(IApplicationDbContext context, IFirmaKoduGenerator firmaKoduGenerator)
    {
        _context = context;
        _firmaKoduGenerator = firmaKoduGenerator;
    }

    public async Task<Result<DukkanResponse>> Handle(CreateDukkanCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        var dukkan = new Dukkan
        {
            Id = Guid.NewGuid(),
            Ad = request.Ad,
            Telefon = request.Telefon,
            Email = request.Email,
            Adres = request.Adres,
            VergiNo = request.VergiNo,
            FirmaKodu = _firmaKoduGenerator.Generate(),
            Aktif = true,
            AbonelikBitisTarihi = request.AbonelikBitisTarihi
        };

        _context.Dukkanlar.Add(dukkan);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new DukkanResponse(
            dukkan.Id,
            dukkan.Ad,
            dukkan.Telefon,
            dukkan.Email,
            dukkan.Adres,
            dukkan.VergiNo,
            dukkan.FirmaKodu,
            dukkan.Aktif,
            dukkan.AbonelikBitisTarihi,
            dukkan.OlusturmaTarihi
        );

        return Result<DukkanResponse>.Succeed(response, "Dükkan başarıyla oluşturuldu");
    }
}