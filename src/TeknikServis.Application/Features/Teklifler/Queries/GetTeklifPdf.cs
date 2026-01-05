using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;

namespace TeknikServis.Application.Features.Teklifler.Queries;

// Query
public record GetTeklifPdfQuery(Guid TeklifId) : IRequest<Result<byte[]>>;

// Handler
public class GetTeklifPdfHandler : IRequestHandler<GetTeklifPdfQuery, Result<byte[]>>
{
    private readonly IApplicationDbContext _context;
    private readonly IPdfService _pdfService;

    public GetTeklifPdfHandler(IApplicationDbContext context, IPdfService pdfService)
    {
        _context = context;
        _pdfService = pdfService;
    }

    public async Task<Result<byte[]>> Handle(GetTeklifPdfQuery query, CancellationToken cancellationToken)
    {
        var teklif = await _context.Teklifler
            .Include(x => x.IsEmri)
            .ThenInclude(x => x.Dukkan)
            .Include(x => x.IsEmri)
            .ThenInclude(x => x.Musteri)
            .Include(x => x.IsEmri)
            .ThenInclude(x => x.Cihaz)
            .ThenInclude(x => x.CihazTanim)
            .Include(x => x.Kalemler)
            .FirstOrDefaultAsync(x => x.Id == query.TeklifId, cancellationToken);

        if (teklif == null)
            return Result<byte[]>.Fail("Teklif bulunamadı");

        var pdfBytes = _pdfService.GenerateTeklifPdf(
            teklif,
            teklif.IsEmri.Musteri,
            teklif.IsEmri.Cihaz,
            teklif.IsEmri.Dukkan
        );

        return Result<byte[]>.Succeed(pdfBytes);
    }
}