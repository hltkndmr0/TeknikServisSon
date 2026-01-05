using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Requests.Cihaz;
using TeknikServis.Domain.Responses.Cihaz;

namespace TeknikServis.Application.Features.Cihazlar.Commands;

// Command
public record CreateCihazTanimCommand(CreateCihazTanimRequest Data) : IRequest<Result<CihazTanimResponse>>;

// Validator
public class CreateCihazTanimValidator : AbstractValidator<CreateCihazTanimCommand>
{
    public CreateCihazTanimValidator()
    {
        RuleFor(x => x.Data.KategoriId)
            .NotEmpty().WithMessage("Kategori zorunludur");

        RuleFor(x => x.Data.Marka)
            .NotEmpty().WithMessage("Marka zorunludur")
            .MaximumLength(100).WithMessage("Marka en fazla 100 karakter olabilir");

        RuleFor(x => x.Data.Model)
            .NotEmpty().WithMessage("Model zorunludur")
            .MaximumLength(100).WithMessage("Model en fazla 100 karakter olabilir");
    }
}

// Handler
public class CreateCihazTanimHandler : IRequestHandler<CreateCihazTanimCommand, Result<CihazTanimResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateCihazTanimHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CihazTanimResponse>> Handle(CreateCihazTanimCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        var kategori = await _context.CihazKategorileri
            .FirstOrDefaultAsync(x => x.Id == request.KategoriId, cancellationToken);

        if (kategori == null)
            return Result<CihazTanimResponse>.Fail("Kategori bulunamadı");

        var cihazTanim = new CihazTanim
        {
            Id = Guid.NewGuid(),
            KategoriId = request.KategoriId,
            Marka = request.Marka,
            Model = request.Model,
            Aktif = true
        };

        _context.CihazTanimlari.Add(cihazTanim);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new CihazTanimResponse(
            cihazTanim.Id,
            cihazTanim.KategoriId,
            kategori.Ad,
            cihazTanim.Marka,
            cihazTanim.Model,
            cihazTanim.Aktif
        );

        return Result<CihazTanimResponse>.Succeed(response, "Cihaz tanımı başarıyla oluşturuldu");
    }
}