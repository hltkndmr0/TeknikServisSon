using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Cihaz;

namespace TeknikServis.Application.Features.Cihazlar.Queries;

// Query
public record GetCihazlarByMusteriQuery(Guid MusteriId) : IRequest<Result<List<CihazResponse>>>;

// Handler
public class GetCihazlarByMusteriHandler : IRequestHandler<GetCihazlarByMusteriQuery, Result<List<CihazResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetCihazlarByMusteriHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<CihazResponse>>> Handle(GetCihazlarByMusteriQuery query, CancellationToken cancellationToken)
    {
        var cihazlar = await _context.Cihazlar
            .Include(x => x.CihazTanim)
            .ThenInclude(x => x.Kategori)
            .Include(x => x.Musteri)
            .Where(x => x.MusteriId == query.MusteriId)
            .OrderByDescending(x => x.OlusturmaTarihi)
            .Select(x => new CihazResponse(
                x.Id,
                x.CihazTanimId,
                x.CihazTanim.Marka,
                x.CihazTanim.Model,
                x.CihazTanim.Kategori.Ad,
                x.MusteriId,
                x.Musteri.AdSoyad ?? x.Musteri.FirmaAdi,
                x.SeriNo,
                x.Imei,
                x.Renk,
                x.GarantiBitisTarihi,
                x.Notlar,
                x.OlusturmaTarihi
            ))
            .ToListAsync(cancellationToken);

        return Result<List<CihazResponse>>.Succeed(cihazlar);
    }
}