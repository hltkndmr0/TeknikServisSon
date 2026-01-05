using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Teklif;

namespace TeknikServis.Application.Features.Teklifler.Queries;

// Query
public record GetTeklifByIdQuery(Guid Id) : IRequest<Result<TeklifResponse>>;

// Handler
public class GetTeklifByIdHandler : IRequestHandler<GetTeklifByIdQuery, Result<TeklifResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetTeklifByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TeklifResponse>> Handle(GetTeklifByIdQuery query, CancellationToken cancellationToken)
    {
        var teklif = await _context.Teklifler
            .Include(x => x.IsEmri)
            .Include(x => x.OlusturanKullanici)
            .Include(x => x.Kalemler)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (teklif == null)
            return Result<TeklifResponse>.Fail("Teklif bulunamadı");

        var kalemResponses = teklif.Kalemler
            .Select(x => new TeklifKalemResponse(
                x.Id,
                x.KalemTipi,
                x.Aciklama,
                x.Miktar,
                x.BirimFiyat,
                x.ToplamFiyat
            ))
            .ToList();

        var response = new TeklifResponse(
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
            kalemResponses
        );

        return Result<TeklifResponse>.Succeed(response);
    }
}