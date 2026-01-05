using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.IsEmri;

namespace TeknikServis.Application.Features.IsEmirleri.Queries;

// Query
public record GetIsEmriByIdQuery(Guid Id) : IRequest<Result<IsEmriDetailResponse>>;

// Handler
public class GetIsEmriByIdHandler : IRequestHandler<GetIsEmriByIdQuery, Result<IsEmriDetailResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetIsEmriByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IsEmriDetailResponse>> Handle(GetIsEmriByIdQuery query, CancellationToken cancellationToken)
    {
        var isEmri = await _context.IsEmirleri
            .Include(x => x.Dukkan)
            .Include(x => x.Musteri)
            .Include(x => x.Cihaz)
            .ThenInclude(x => x.CihazTanim)
            .ThenInclude(x => x.Kategori)
            .Include(x => x.Durum)
            .Include(x => x.TeslimAlanKullanici)
            .Include(x => x.AtananKullanici)
            .Include(x => x.IsNotlari)
            .ThenInclude(x => x.Kullanici)
            .Include(x => x.DurumGecmisi)
            .ThenInclude(x => x.Durum)
            .Include(x => x.DurumGecmisi)
            .ThenInclude(x => x.Kullanici)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (isEmri == null)
            return Result<IsEmriDetailResponse>.Fail("İş emri bulunamadı");

        var notlar = isEmri.IsNotlari
            .OrderByDescending(x => x.OlusturmaTarihi)
            .Select(x => new IsNotuResponse(
                x.Id,
                x.KullaniciId,
                x.Kullanici.AdSoyad,
                x.NotMetni,
                x.OlusturmaTarihi
            ))
            .ToList();

        var durumGecmisi = isEmri.DurumGecmisi
            .OrderByDescending(x => x.DegisimTarihi)
            .Select(x => new IsDurumGecmisiResponse(
                x.Id,
                x.Durum.Ad,
                x.Durum.Renk,
                x.Kullanici.AdSoyad,
                x.Aciklama,
                x.DegisimTarihi
            ))
            .ToList();

        var response = new IsEmriDetailResponse(
            isEmri.Id,
            isEmri.IsEmriNo,
            isEmri.DukkanId,
            isEmri.Dukkan.Ad,
            isEmri.MusteriId,
            isEmri.Musteri.AdSoyad ?? isEmri.Musteri.FirmaAdi ?? "",
            isEmri.Musteri.Telefon,
            isEmri.Musteri.Email,
            isEmri.CihazId,
            $"{isEmri.Cihaz.CihazTanim.Marka} {isEmri.Cihaz.CihazTanim.Model}",
            isEmri.Cihaz.SeriNo,
            isEmri.Cihaz.Imei,
            isEmri.DurumId,
            isEmri.Durum.Ad,
            isEmri.Durum.Renk,
            isEmri.TeslimAlanKullaniciId,
            isEmri.TeslimAlanKullanici.AdSoyad,
            isEmri.AtananKullaniciId,
            isEmri.AtananKullanici?.AdSoyad,
            isEmri.ArizaAciklamasi,
            isEmri.OnTeshis,
            isEmri.YapilanIslem,
            isEmri.TahminiSureGun,
            isEmri.TahminiUcret,
            isEmri.KesinUcret,
            isEmri.GarantiKapsaminda,
            isEmri.Oncelik,
            isEmri.TeslimTarihi,
            isEmri.TamamlanmaTarihi,
            isEmri.OlusturmaTarihi,
            notlar,
            durumGecmisi
        );

        return Result<IsEmriDetailResponse>.Succeed(response);
    }
}