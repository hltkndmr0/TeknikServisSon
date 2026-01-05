using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Requests.Dukkan;

namespace TeknikServis.Application.Features.Dukkanlar.Commands;

public record UpdateDukkanCommand(UpdateDukkanRequest Request) : IRequest<Result<bool>>;

public class UpdateDukkanHandler : IRequestHandler<UpdateDukkanCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateDukkanHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(UpdateDukkanCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var dukkan = await _context.Dukkanlar
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (dukkan == null)
            return Result<bool>.Fail("Dükkan bulunamadı");

        // Güncelle
        dukkan.Ad = request.Ad;
        dukkan.Telefon = request.Telefon;
        dukkan.Email = request.Email;
        dukkan.Adres = request.Adres;
        dukkan.VergiNo = request.VergiNo;
        dukkan.Aktif = request.Aktif;
        dukkan.AbonelikBitisTarihi = request.AbonelikBitisTarihi;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Succeed(true, "Dükkan güncellendi");
    }
}