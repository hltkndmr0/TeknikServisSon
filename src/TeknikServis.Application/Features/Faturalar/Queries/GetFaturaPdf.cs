using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;

namespace TeknikServis.Application.Features.Faturalar.Queries;

// Query
public record GetFaturaPdfQuery(Guid FaturaId) : IRequest<Result<byte[]>>;

// Handler
public class GetFaturaPdfHandler : IRequestHandler<GetFaturaPdfQuery, Result<byte[]>>
{
    private readonly IApplicationDbContext _context;
    private readonly IPdfService _pdfService;

    public GetFaturaPdfHandler(IApplicationDbContext context, IPdfService pdfService)
    {
        _context = context;
        _pdfService = pdfService;
    }

    public async Task<Result<byte[]>> Handle(GetFaturaPdfQuery query, CancellationToken cancellationToken)
    {
        var fatura = await _context.Faturalar
            .Include(x => x.IsEmri)
            .ThenInclude(x => x.Dukkan)
            .Include(x => x.IsEmri)
            .ThenInclude(x => x.Musteri)
            .Include(x => x.IsEmri)
            .ThenInclude(x => x.Cihaz)
            .ThenInclude(x => x.CihazTanim)
            .Include(x => x.Kalemler)
            .FirstOrDefaultAsync(x => x.Id == query.FaturaId, cancellationToken);

        if (fatura == null)
            return Result<byte[]>.Fail("Fatura bulunamadı");

        var pdfBytes = _pdfService.GenerateFaturaPdf(
            fatura,
            fatura.IsEmri.Musteri,
            fatura.IsEmri.Cihaz,
            fatura.IsEmri.Dukkan
        );

        return Result<byte[]>.Succeed(pdfBytes);
    }
}