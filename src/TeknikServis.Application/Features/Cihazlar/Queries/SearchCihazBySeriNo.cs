using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Cihaz;

namespace TeknikServis.Application.Features.Cihazlar.Queries;

// Query
public record SearchCihazBySeriNoQuery(string SeriNo) : IRequest<Result<CihazResponse>>;

// Handler
public class SearchCihazBySeriNoHandler : IRequestHandler<SearchCihazBySeriNoQuery, Result<CihazResponse>>
{
    private readonly IApplicationDbContext _context;

    public SearchCihazBySeriNoHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CihazResponse>> Handle(SearchCihazBySeriNoQuery query, CancellationToken cancellationToken)
    {
        var cihaz = await _context.Cihazlar
            .Include(x => x.CihazTanim)
            .ThenInclude(x => x.Kategori)
            .Include(x => x.Musteri)
            .FirstOrDefaultAsync(x => x.SeriNo == query.SeriNo, cancellationToken);

        if (cihaz == null)
            return Result<CihazResponse>.Fail("Bu seri numarasına ait cihaz bulunamadı");

        var response = new CihazResponse(
            cihaz.Id,
            cihaz.CihazTanimId,
            cihaz.CihazTanim.Marka,
            cihaz.CihazTanim.Model,
            cihaz.CihazTanim.Kategori.Ad,
            cihaz.MusteriId,
            cihaz.Musteri.AdSoyad ?? cihaz.Musteri.FirmaAdi,
            cihaz.SeriNo,
            cihaz.Imei,
            cihaz.Renk,
            cihaz.GarantiBitisTarihi,
            cihaz.Notlar,
            cihaz.OlusturmaTarihi
        );

        return Result<CihazResponse>.Succeed(response);
    }
}