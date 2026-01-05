using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Responses.Musteri;

namespace TeknikServis.Application.Features.Musteriler.Queries;

// Query
public record GetMusteriByIdQuery(Guid Id) : IRequest<Result<MusteriResponse>>;

// Handler
public class GetMusteriByIdHandler : IRequestHandler<GetMusteriByIdQuery, Result<MusteriResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetMusteriByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<MusteriResponse>> Handle(GetMusteriByIdQuery query, CancellationToken cancellationToken)
    {
        var musteri = await _context.Musteriler
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (musteri == null)
            return Result<MusteriResponse>.Fail("Müşteri bulunamadı");

        var response = new MusteriResponse(
            musteri.Id,
            musteri.MusteriTipi,
            musteri.AdSoyad,
            musteri.FirmaAdi,
            musteri.VergiNo,
            musteri.Telefon,
            musteri.Telefon2,
            musteri.Email,
            musteri.Adres,
            musteri.Notlar,
            musteri.OlusturmaTarihi
        );

        return Result<MusteriResponse>.Succeed(response);
    }
}