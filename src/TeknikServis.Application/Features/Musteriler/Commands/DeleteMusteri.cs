using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;

namespace TeknikServis.Application.Features.Musteriler.Commands;

public record DeleteMusteriCommand(Guid Id) : IRequest<Result<bool>>;

public class DeleteMusteriHandler : IRequestHandler<DeleteMusteriCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteMusteriHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(DeleteMusteriCommand command, CancellationToken cancellationToken)
    {
        var musteri = await _context.Musteriler
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (musteri == null)
            return Result<bool>.Fail("Müşteri bulunamadı");

        // İş emri kontrolü
        var isEmriVar = await _context.IsEmirleri
            .AnyAsync(x => x.MusteriId == command.Id, cancellationToken);

        if (isEmriVar)
            return Result<bool>.Fail("Bu müşteriye ait iş emirleri bulunduğu için silinemez");

        // Cihaz kontrolü
        var cihazVar = await _context.Cihazlar
            .AnyAsync(x => x.MusteriId == command.Id, cancellationToken);

        if (cihazVar)
            return Result<bool>.Fail("Bu müşteriye ait cihazlar bulunduğu için silinemez");

        // DukkanMusteri ilişkisini sil
        var dukkanMusteri = await _context.DukkanMusterileri
            .Where(x => x.MusteriId == command.Id)
            .ToListAsync(cancellationToken);

        _context.DukkanMusterileri.RemoveRange(dukkanMusteri);
        
        // Müşteriyi sil
        _context.Musteriler.Remove(musteri);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Succeed(true, "Müşteri silindi");
    }
}