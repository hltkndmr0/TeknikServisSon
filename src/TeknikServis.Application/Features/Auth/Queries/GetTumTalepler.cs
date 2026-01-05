using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Enums;
using TeknikServis.Domain.Responses.Auth;
using TeknikServis.Domain.Responses.Common;

namespace TeknikServis.Application.Features.Auth.Queries;

// Request
public record GetTumTaleplerRequest(
    KayitTalepDurumu? Durum,
    int Page = 1,
    int PageSize = 20
);

// Query
public record GetTumTaleplerQuery(GetTumTaleplerRequest Data) : IRequest<Result<PaginatedResponse<KayitTalepResponse>>>;

// Handler
public class GetTumTaleplerHandler : IRequestHandler<GetTumTaleplerQuery, Result<PaginatedResponse<KayitTalepResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetTumTaleplerHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedResponse<KayitTalepResponse>>> Handle(GetTumTaleplerQuery query, CancellationToken cancellationToken)
    {
        var request = query.Data;

        var queryable = _context.KayitTalepleri.AsQueryable();

        if (request.Durum.HasValue)
            queryable = queryable.Where(x => x.Durum == request.Durum.Value);

        var totalCount = await queryable.CountAsync(cancellationToken);

        var talepler = await queryable
            .OrderByDescending(x => x.TalepTarihi)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new KayitTalepResponse(
                x.Id,
                x.FirmaAdi,
                x.FirmaTelefon,
                x.FirmaEmail,
                x.FirmaAdres,
                x.VergiNo,
                x.YetkiliAdSoyad,
                x.YetkiliTelefon,
                x.YetkiliEmail,
                x.Durum,
                x.RedNedeni,
                x.TalepTarihi,
                x.IslemTarihi,
                x.OlusturulanFirmaKodu,
                x.OlusturulanSifre
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var response = new PaginatedResponse<KayitTalepResponse>(
            talepler,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        );

        return Result<PaginatedResponse<KayitTalepResponse>>.Succeed(response);
    }
}