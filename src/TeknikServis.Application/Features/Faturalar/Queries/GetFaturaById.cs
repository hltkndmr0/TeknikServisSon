using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Fatura;

namespace TeknikServis.Application.Features.Faturalar.Queries;

// Query
public record GetFaturaByIdQuery(Guid Id) : IRequest<Result<FaturaResponse>>;

// Handler
public class GetFaturaByIdHandler : IRequestHandler<GetFaturaByIdQuery, Result<FaturaResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetFaturaByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<FaturaResponse>> Handle(GetFaturaByIdQuery query, CancellationToken cancellationToken)
    {
        var fatura = await _context.Faturalar
            .Include(x => x.IsEmri)
            .Include(x => x.OlusturanKullanici)
            .Include(x => x.Kalemler)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (fatura == null)
            return Result<FaturaResponse>.Fail("Fatura bulunamadı");

        var kalemResponses = fatura.Kalemler
            .Select(x => new FaturaKalemResponse(
                x.Id,
                x.KalemTipi,
                x.Aciklama,
                x.Miktar,
                x.BirimFiyat,
                x.ToplamFiyat
            ))
            .ToList();

        var response = new FaturaResponse(
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
            kalemResponses
        );

        return Result<FaturaResponse>.Succeed(response);
    }
}