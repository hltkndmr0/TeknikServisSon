using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Requests.Cihaz;
using TeknikServis.Domain.Responses.Cihaz;
using TeknikServis.Domain.Responses.Common;

namespace TeknikServis.Application.Features.Cihazlar.Queries;

// Query
public record GetCihazlarQuery(GetCihazlarRequest Request) : IRequest<Result<PaginatedResponse<CihazListResponse>>>;

// Handler
public class GetCihazlarHandler : IRequestHandler<GetCihazlarQuery, Result<PaginatedResponse<CihazListResponse>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetCihazlarHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<PaginatedResponse<CihazListResponse>>> Handle(GetCihazlarQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;

        var baseQuery = _context.Cihazlar
            .Include(c => c.CihazTanim)
                .ThenInclude(t => t.Kategori)
            .Include(c => c.Musteri)
            .Where(c => c.Musteri.DukkanMusterileri.Any(dm => dm.DukkanId == _currentUser.DukkanId))
            .AsQueryable();

        // Arama
        if (!string.IsNullOrWhiteSpace(request.Arama))
        {
            var arama = request.Arama.ToLower();
            baseQuery = baseQuery.Where(c =>
                (c.SeriNo != null && c.SeriNo.ToLower().Contains(arama)) ||
                (c.Imei != null && c.Imei.ToLower().Contains(arama)) ||
                c.CihazTanim.Marka.ToLower().Contains(arama) ||
                c.CihazTanim.Model.ToLower().Contains(arama) ||
                (c.Musteri.AdSoyad != null && c.Musteri.AdSoyad.ToLower().Contains(arama)) ||
                (c.Musteri.FirmaAdi != null && c.Musteri.FirmaAdi.ToLower().Contains(arama))
            );
        }

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var items = await baseQuery
            .OrderByDescending(c => c.OlusturmaTarihi)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CihazListResponse
            {
                Id = c.Id,
                MusteriId = c.MusteriId,
                MusteriAd = c.Musteri.AdSoyad ?? c.Musteri.FirmaAdi ?? "",
                CihazTanimId = c.CihazTanimId,
                KategoriAd = c.CihazTanim.Kategori.Ad,
                Marka = c.CihazTanim.Marka,
                Model = c.CihazTanim.Model,
                SeriNo = c.SeriNo,
                Imei = c.Imei,
                Renk = c.Renk,
                OlusturmaTarihi = c.OlusturmaTarihi
            })
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var response = new PaginatedResponse<CihazListResponse>(
            items,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        );

        return Result<PaginatedResponse<CihazListResponse>>.Succeed(response);
    }
}