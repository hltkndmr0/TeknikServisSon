using FluentValidation;
using MediatR;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Requests.Cihaz;
using TeknikServis.Domain.Responses.Cihaz;

namespace TeknikServis.Application.Features.Cihazlar.Commands;

// Command
public record CreateCihazKategoriCommand(CreateCihazKategoriRequest Data) : IRequest<Result<CihazKategoriResponse>>;

// Validator
public class CreateCihazKategoriValidator : AbstractValidator<CreateCihazKategoriCommand>
{
    public CreateCihazKategoriValidator()
    {
        RuleFor(x => x.Data.Ad)
            .NotEmpty().WithMessage("Kategori adı zorunludur")
            .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter olabilir");
    }
}

// Handler
public class CreateCihazKategoriHandler : IRequestHandler<CreateCihazKategoriCommand, Result<CihazKategoriResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateCihazKategoriHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CihazKategoriResponse>> Handle(CreateCihazKategoriCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        var kategori = new CihazKategori
        {
            Id = Guid.NewGuid(),
            Ad = request.Ad,
            Aktif = true
        };

        _context.CihazKategorileri.Add(kategori);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new CihazKategoriResponse(
            kategori.Id,
            kategori.Ad,
            kategori.Aktif
        );

        return Result<CihazKategoriResponse>.Succeed(response, "Kategori başarıyla oluşturuldu");
    }
}