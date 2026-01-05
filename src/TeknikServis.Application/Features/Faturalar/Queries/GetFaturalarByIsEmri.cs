using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Fatura;

namespace TeknikServis.Application.Features.Faturalar.Queries;

// Query
public record GetFaturalarByIsEmriQuery(Guid IsEmriId) : IRequest<Result<List<FaturaResponse>>>;

// Handler
public class GetFaturalarByIsEmriHandler : IRequestHandler<GetFaturalarByIsEmriQuery, Result<List<FaturaResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetFaturalarByIsEmriHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<FaturaResponse>>> Handle(GetFaturalarByIsEmriQuery query, CancellationToken cancellationToken)
    {
        var faturalar = await _context.Faturalar
            .Include(x => x.IsEmri)
            .Include(x => x.OlusturanKullanici)
            .Include(x => x.Kalemler)
            .Where(x => x.IsEmriId == query.IsEmriId)
            .OrderByDescending(x => x.OlusturmaTarihi)
            .ToListAsync(cancellationToken);

        var response = faturalar.Select(fatura => new FaturaResponse(
            fatura.Id,
            fatura.FaturaNo,
            fatura.IsEmriId,
            fatura.IsEmri.IsEmriNo,
            fatura.TeklifId,
            fatura.FaturaTarihi,
            fatura.ToplamTutar,
            fatura.KdvOrani,
            fatura.KdvTutar,
            fatura.GenelToplam,
            fatura.Aciklama,
            fatura.OlusturanKullaniciId,
            fatura.OlusturanKullanici.AdSoyad,
            fatura.OlusturmaTarihi,
            fatura.Kalemler.Select(k => new FaturaKalemResponse(
                k.Id,
                k.KalemTipi,
                k.Aciklama,
                k.Miktar,
                k.BirimFiyat,
                k.ToplamFiyat
            )).ToList()
        )).ToList();

        return Result<List<FaturaResponse>>.Succeed(response);
    }
}