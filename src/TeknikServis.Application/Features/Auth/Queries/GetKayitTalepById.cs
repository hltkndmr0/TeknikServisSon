using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Auth;

namespace TeknikServis.Application.Features.Auth.Queries;

// Query
public record GetKayitTalepByIdQuery(Guid Id) : IRequest<Result<KayitTalepResponse>>;

// Handler
public class GetKayitTalepByIdHandler : IRequestHandler<GetKayitTalepByIdQuery, Result<KayitTalepResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetKayitTalepByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<KayitTalepResponse>> Handle(GetKayitTalepByIdQuery query, CancellationToken cancellationToken)
    {
        var kayitTalep = await _context.KayitTalepleri
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (kayitTalep == null)
            return Result<KayitTalepResponse>.Fail("Kayıt talebi bulunamadı");

        var response = new KayitTalepResponse(
            kayitTalep.Id,
            kayitTalep.FirmaAdi,
            kayitTalep.FirmaTelefon,
            kayitTalep.FirmaEmail,
            kayitTalep.FirmaAdres,
            kayitTalep.VergiNo,
            kayitTalep.YetkiliAdSoyad,
            kayitTalep.YetkiliTelefon,
            kayitTalep.YetkiliEmail,
            kayitTalep.Durum,
            kayitTalep.RedNedeni,
            kayitTalep.TalepTarihi,
            kayitTalep.IslemTarihi,
            kayitTalep.OlusturulanFirmaKodu,
            kayitTalep.OlusturulanSifre
        );

        return Result<KayitTalepResponse>.Succeed(response);
    }
}