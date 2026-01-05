using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Requests.Musteri;
using TeknikServis.Domain.Responses.Common;
using TeknikServis.Domain.Responses.Musteri;

namespace TeknikServis.Application.Features.Musteriler.Queries;

// Query
public record SearchMusteriQuery(SearchMusteriRequest Data) : IRequest<Result<PaginatedResponse<MusteriResponse>>>;

// Handler
public class SearchMusteriHandler : IRequestHandler<SearchMusteriQuery, Result<PaginatedResponse<MusteriResponse>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public SearchMusteriHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PaginatedResponse<MusteriResponse>>> Handle(SearchMusteriQuery query, CancellationToken cancellationToken)
    {
        var request = query.Data;

        if (!_currentUserService.DukkanId.HasValue)
            return Result<PaginatedResponse<MusteriResponse>>.Fail("Dükkan bilgisi bulunamadı");

        var dukkanId = _currentUserService.DukkanId.Value;

        var queryable = _context.DukkanMusterileri
            .Where(x => x.DukkanId == dukkanId)
            .Select(x => x.Musteri);

        if (!string.IsNullOrEmpty(request.Telefon))
            queryable = queryable.Where(x => x.Telefon.Contains(request.Telefon));

        if (!string.IsNullOrEmpty(request.AdSoyad))
            queryable = queryable.Where(x => x.AdSoyad != null && x.AdSoyad.Contains(request.AdSoyad));

        if (!string.IsNullOrEmpty(request.FirmaAdi))
            queryable = queryable.Where(x => x.FirmaAdi != null && x.FirmaAdi.Contains(request.FirmaAdi));

        var totalCount = await queryable.CountAsync(cancellationToken);

        var musteriler = await queryable
            .OrderByDescending(x => x.OlusturmaTarihi)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new MusteriResponse(
                x.Id,
                x.MusteriTipi,
                x.AdSoyad,
                x.FirmaAdi,
                x.VergiNo,
                x.Telefon,
                x.Telefon2,
                x.Email,
                x.Adres,
                x.Notlar,
                x.OlusturmaTarihi
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var response = new PaginatedResponse<MusteriResponse>(
            musteriler,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        );

        return Result<PaginatedResponse<MusteriResponse>>.Succeed(response);
    }
}