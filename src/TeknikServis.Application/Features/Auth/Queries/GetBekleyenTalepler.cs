using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Enums;
using TeknikServis.Domain.Responses.Auth;

namespace TeknikServis.Application.Features.Auth.Queries;

// Query
public record GetBekleyenTaleplerQuery() : IRequest<Result<List<KayitTalepResponse>>>;

// Handler
public class GetBekleyenTaleplerHandler : IRequestHandler<GetBekleyenTaleplerQuery, Result<List<KayitTalepResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetBekleyenTaleplerHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<KayitTalepResponse>>> Handle(GetBekleyenTaleplerQuery query, CancellationToken cancellationToken)
    {
        var talepler = await _context.KayitTalepleri
            .Where(x => x.Durum == KayitTalepDurumu.Beklemede)
            .OrderByDescending(x => x.TalepTarihi)
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

        return Result<List<KayitTalepResponse>>.Succeed(talepler);
    }
}