using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Dukkan;

namespace TeknikServis.Application.Features.Dukkanlar.Queries;

// Query
public record GetDukkanByIdQuery(Guid Id) : IRequest<Result<DukkanResponse>>;

// Handler
public class GetDukkanByIdHandler : IRequestHandler<GetDukkanByIdQuery, Result<DukkanResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetDukkanByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DukkanResponse>> Handle(GetDukkanByIdQuery query, CancellationToken cancellationToken)
    {
        var dukkan = await _context.Dukkanlar
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (dukkan == null)
            return Result<DukkanResponse>.Fail("Dükkan bulunamadı");

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

        return Result<DukkanResponse>.Succeed(response);
    }
}