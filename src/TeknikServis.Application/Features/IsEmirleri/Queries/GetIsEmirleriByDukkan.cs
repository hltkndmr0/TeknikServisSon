using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Enums;
using TeknikServis.Domain.Responses.Common;
using TeknikServis.Domain.Responses.IsEmri;

namespace TeknikServis.Application.Features.IsEmirleri.Queries;

// Request
public record GetIsEmirleriByDukkanRequest(
    Guid? DurumId,
    Oncelik? Oncelik,
    string? Arama,
    int Page = 1,
    int PageSize = 20
);

// Query
public record GetIsEmirleriByDukkanQuery(GetIsEmirleriByDukkanRequest Data) : IRequest<Result<PaginatedResponse<IsEmriResponse>>>;

// Handler
public class GetIsEmirleriByDukkanHandler : IRequestHandler<GetIsEmirleriByDukkanQuery, Result<PaginatedResponse<IsEmriResponse>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetIsEmirleriByDukkanHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PaginatedResponse<IsEmriResponse>>> Handle(GetIsEmirleriByDukkanQuery query, CancellationToken cancellationToken)
    {
        var request = query.Data;

        if (!_currentUserService.DukkanId.HasValue)
            return Result<PaginatedResponse<IsEmriResponse>>.Fail("Dükkan bilgisi bulunamadı");

        var dukkanId = _currentUserService.DukkanId.Value;

        var queryable = _context.IsEmirleri
            .Include(x => x.Musteri)
            .Include(x => x.Cihaz)
            .ThenInclude(x => x.CihazTanim)
            .Include(x => x.Durum)
            .Where(x => x.DukkanId == dukkanId)
            .AsQueryable();

        if (request.DurumId.HasValue)
            queryable = queryable.Where(x => x.DurumId == request.DurumId.Value);

        if (request.Oncelik.HasValue)
            queryable = queryable.Where(x => x.Oncelik == request.Oncelik.Value);

        if (!string.IsNullOrEmpty(request.Arama))
        {
            queryable = queryable.Where(x =>
                x.IsEmriNo.Contains(request.Arama) ||
                x.Musteri.Telefon.Contains(request.Arama) ||
                (x.Musteri.AdSoyad != null && x.Musteri.AdSoyad.Contains(request.Arama)) ||
                (x.Cihaz.SeriNo != null && x.Cihaz.SeriNo.Contains(request.Arama)));
        }

        var totalCount = await queryable.CountAsync(cancellationToken);

        var isEmirleri = await queryable
            .OrderByDescending(x => x.OlusturmaTarihi)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new IsEmriResponse(
                x.Id,
                x.IsEmriNo,
                x.DukkanId,
                x.MusteriId,
                x.Musteri.AdSoyad ?? x.Musteri.FirmaAdi ?? "",
                x.Musteri.Telefon,
                x.CihazId,
                x.Cihaz.CihazTanim.Marka + " " + x.Cihaz.CihazTanim.Model,
                x.Cihaz.SeriNo,
                x.DurumId,
                x.Durum.Ad,
                x.Durum.Renk,
                x.ArizaAciklamasi,
                x.Oncelik,
                x.TahminiUcret,
                x.OlusturmaTarihi
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var response = new PaginatedResponse<IsEmriResponse>(
            isEmirleri,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        );

        return Result<PaginatedResponse<IsEmriResponse>>.Succeed(response);
    }
}