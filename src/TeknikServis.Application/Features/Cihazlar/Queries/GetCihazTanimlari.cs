using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Cihaz;

namespace TeknikServis.Application.Features.Cihazlar.Queries;

// Request
public record GetCihazTanimlariRequest(
    Guid? KategoriId,
    string? Arama
);

// Query
public record GetCihazTanimlariQuery(GetCihazTanimlariRequest Data) : IRequest<Result<List<CihazTanimResponse>>>;

// Handler
public class GetCihazTanimlariHandler : IRequestHandler<GetCihazTanimlariQuery, Result<List<CihazTanimResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetCihazTanimlariHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<CihazTanimResponse>>> Handle(GetCihazTanimlariQuery query, CancellationToken cancellationToken)
    {
        var request = query.Data;

        var queryable = _context.CihazTanimlari
            .Include(x => x.Kategori)
            .Where(x => x.Aktif)
            .AsQueryable();

        if (request.KategoriId.HasValue)
            queryable = queryable.Where(x => x.KategoriId == request.KategoriId.Value);

        if (!string.IsNullOrEmpty(request.Arama))
        {
            queryable = queryable.Where(x =>
                x.Marka.Contains(request.Arama) ||
                x.Model.Contains(request.Arama));
        }

        var tanimlar = await queryable
            .OrderBy(x => x.Marka)
            .ThenBy(x => x.Model)
            .Select(x => new CihazTanimResponse(
                x.Id,
                x.KategoriId,
                x.Kategori.Ad,
                x.Marka,
                x.Model,
                x.Aktif
            ))
            .ToListAsync(cancellationToken);

        return Result<List<CihazTanimResponse>>.Succeed(tanimlar);
    }
}