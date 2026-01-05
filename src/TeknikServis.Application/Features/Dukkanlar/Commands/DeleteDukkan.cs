using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;

namespace TeknikServis.Application.Features.Dukkanlar.Commands;

// Command
public record DeleteDukkanCommand(Guid Id) : IRequest<Result<bool>>;

// Validator
public class DeleteDukkanValidator : AbstractValidator<DeleteDukkanCommand>
{
    public DeleteDukkanValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Dükkan ID zorunludur");
    }
}

// Handler
public class DeleteDukkanHandler : IRequestHandler<DeleteDukkanCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteDukkanHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(DeleteDukkanCommand command, CancellationToken cancellationToken)
    {
        var dukkan = await _context.Dukkanlar
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (dukkan == null)
            return Result<bool>.Fail("Dükkan bulunamadı");

        // Soft delete - aktifliği kapat
        dukkan.Aktif = false;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Succeed(true, "Dükkan başarıyla silindi");
    }
}