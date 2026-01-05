using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Teklif;

namespace TeknikServis.Application.Features.Teklifler.Queries;

// Query
public record GetTekliflerByIsEmriQuery(Guid IsEmriId) : IRequest<Result<List<TeklifResponse>>>;

// Handler
public class GetTekliflerByIsEmriHandler : IRequestHandler<GetTekliflerByIsEmriQuery, Result<List<TeklifResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetTekliflerByIsEmriHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<TeklifResponse>>> Handle(GetTekliflerByIsEmriQuery query, CancellationToken cancellationToken)
    {
        var teklifler = await _context.Teklifler
            .Include(x => x.IsEmri)
            .Include(x => x.OlusturanKullanici)
            .Include(x => x.Kalemler)
            .Where(x => x.IsEmriId == query.IsEmriId)
            .OrderByDescending(x => x.OlusturmaTarihi)
            .ToListAsync(cancellationToken);

        var response = teklifler.Select(teklif => new TeklifResponse(
            teklif.Id,
            teklif.TeklifNo,
            teklif.IsEmriId,
            teklif.IsEmri.IsEmriNo,
            teklif.Durum,
            teklif.ToplamTutar,
            teklif.Aciklama,
            teklif.GecerlilikTarihi,
            teklif.OnayTarihi,
            teklif.MailGonderildi,
            teklif.MailGonderimTarihi,
            teklif.OlusturanKullaniciId,
            teklif.OlusturanKullanici.AdSoyad,
            teklif.OlusturmaTarihi,
            teklif.Kalemler.Select(k => new TeklifKalemResponse(
                k.Id,
                k.KalemTipi,
                k.Aciklama,
                k.Miktar,
                k.BirimFiyat,
                k.ToplamFiyat
            )).ToList()
        )).ToList();

        return Result<List<TeklifResponse>>.Succeed(response);
    }
}