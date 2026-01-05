using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.IsEmri;

namespace TeknikServis.Application.Features.IsEmirleri.Queries;

// Query
public record GetIsDurumlariQuery() : IRequest<Result<List<IsDurumuResponse>>>;

// Handler
public class GetIsDurumlariHandler : IRequestHandler<GetIsDurumlariQuery, Result<List<IsDurumuResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetIsDurumlariHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<IsDurumuResponse>>> Handle(GetIsDurumlariQuery query, CancellationToken cancellationToken)
    {
        var durumlar = await _context.IsDurumlari
            .Where(x => x.Aktif)
            .OrderBy(x => x.Sira)
            .Select(x => new IsDurumuResponse(
                x.Id,
                x.Ad,
                x.Renk,
                x.Sira
            ))
            .ToListAsync(cancellationToken);

        return Result<List<IsDurumuResponse>>.Succeed(durumlar);
    }
}