using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.IsEmri;

namespace TeknikServis.Application.Features.IsEmirleri.Queries;

// Query
public record GetIsEmirleriByDurumQuery(Guid DurumId) : IRequest<Result<List<IsEmriResponse>>>;

// Handler
public class GetIsEmirleriByDurumHandler : IRequestHandler<GetIsEmirleriByDurumQuery, Result<List<IsEmriResponse>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetIsEmirleriByDurumHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<IsEmriResponse>>> Handle(GetIsEmirleriByDurumQuery query, CancellationToken cancellationToken)
    {
        if (!_currentUserService.DukkanId.HasValue)
            return Result<List<IsEmriResponse>>.Fail("Dükkan bilgisi bulunamadı");

        var dukkanId = _currentUserService.DukkanId.Value;

        var isEmirleri = await _context.IsEmirleri
            .Include(x => x.Musteri)
            .Include(x => x.Cihaz)
            .ThenInclude(x => x.CihazTanim)
            .Include(x => x.Durum)
            .Where(x => x.DukkanId == dukkanId && x.DurumId == query.DurumId)
            .OrderByDescending(x => x.OlusturmaTarihi)
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

        return Result<List<IsEmriResponse>>.Succeed(isEmirleri);
    }
}