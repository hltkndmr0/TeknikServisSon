using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Cihaz;

namespace TeknikServis.Application.Features.Cihazlar.Queries;

// Query
public record GetCihazKategorileriQuery() : IRequest<Result<List<CihazKategoriResponse>>>;

// Handler
public class GetCihazKategorileriHandler : IRequestHandler<GetCihazKategorileriQuery, Result<List<CihazKategoriResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetCihazKategorileriHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<CihazKategoriResponse>>> Handle(GetCihazKategorileriQuery query, CancellationToken cancellationToken)
    {
        var kategoriler = await _context.CihazKategorileri
            .Where(x => x.Aktif)
            .OrderBy(x => x.Ad)
            .Select(x => new CihazKategoriResponse(
                x.Id,
                x.Ad,
                x.Aktif
            ))
            .ToListAsync(cancellationToken);

        return Result<List<CihazKategoriResponse>>.Succeed(kategoriler);
    }
}