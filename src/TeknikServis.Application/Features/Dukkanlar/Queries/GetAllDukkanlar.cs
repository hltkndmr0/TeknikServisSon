using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Common;
using TeknikServis.Domain.Responses.Dukkan;

namespace TeknikServis.Application.Features.Dukkanlar.Queries;

// Request
public record GetAllDukkanlarRequest(
    string? Arama,
    bool? Aktif,
    int Page = 1,
    int PageSize = 20
);

// Query
public record GetAllDukkanlarQuery(GetAllDukkanlarRequest Data) : IRequest<Result<PaginatedResponse<DukkanResponse>>>;

// Handler
public class GetAllDukkanlarHandler : IRequestHandler<GetAllDukkanlarQuery, Result<PaginatedResponse<DukkanResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllDukkanlarHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedResponse<DukkanResponse>>> Handle(GetAllDukkanlarQuery query, CancellationToken cancellationToken)
    {
        var request = query.Data;

        var queryable = _context.Dukkanlar.AsQueryable();

        if (!string.IsNullOrEmpty(request.Arama))
        {
            queryable = queryable.Where(x =>
                x.Ad.Contains(request.Arama) ||
                x.FirmaKodu!.Contains(request.Arama) ||
                x.Telefon!.Contains(request.Arama));
        }

        if (request.Aktif.HasValue)
            queryable = queryable.Where(x => x.Aktif == request.Aktif.Value);

        var totalCount = await queryable.CountAsync(cancellationToken);

        var dukkanlar = await queryable
            .OrderByDescending(x => x.OlusturmaTarihi)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new DukkanResponse(
                x.Id,
                x.Ad,
                x.Telefon,
                x.Email,
                x.Adres,
                x.VergiNo,
                x.FirmaKodu,
                x.Aktif,
                x.AbonelikBitisTarihi,
                x.OlusturmaTarihi
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var response = new PaginatedResponse<DukkanResponse>(
            dukkanlar,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        );

        return Result<PaginatedResponse<DukkanResponse>>.Succeed(response);
    }
}