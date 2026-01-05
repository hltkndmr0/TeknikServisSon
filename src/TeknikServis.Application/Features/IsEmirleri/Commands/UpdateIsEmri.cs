using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Requests.IsEmri;
using TeknikServis.Domain.Responses.IsEmri;

namespace TeknikServis.Application.Features.IsEmirleri.Commands;

// Command
public record UpdateIsEmriCommand(UpdateIsEmriRequest Data) : IRequest<Result<IsEmriResponse>>;

// Validator
public class UpdateIsEmriValidator : AbstractValidator<UpdateIsEmriCommand>
{
    public UpdateIsEmriValidator()
    {
        RuleFor(x => x.Data.Id)
            .NotEmpty().WithMessage("İş emri ID zorunludur");
    }
}

// Handler
public class UpdateIsEmriHandler : IRequestHandler<UpdateIsEmriCommand, Result<IsEmriResponse>>
{
    private readonly IApplicationDbContext _context;

    public UpdateIsEmriHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IsEmriResponse>> Handle(UpdateIsEmriCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        var isEmri = await _context.IsEmirleri
            .Include(x => x.Musteri)
            .Include(x => x.Cihaz)
            .ThenInclude(x => x.CihazTanim)
            .Include(x => x.Durum)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (isEmri == null)
            return Result<IsEmriResponse>.Fail("İş emri bulunamadı");

        isEmri.ArizaAciklamasi = request.ArizaAciklamasi;
        isEmri.OnTeshis = request.OnTeshis;
        isEmri.YapilanIslem = request.YapilanIslem;
        isEmri.TahminiSureGun = request.TahminiSureGun;
        isEmri.TahminiUcret = request.TahminiUcret;
        isEmri.KesinUcret = request.KesinUcret;
        isEmri.GarantiKapsaminda = request.GarantiKapsaminda;
        isEmri.Oncelik = request.Oncelik;
        isEmri.AtananKullaniciId = request.AtananKullaniciId;

        await _context.SaveChangesAsync(cancellationToken);

        var response = new IsEmriResponse(
            isEmri.Id,
            isEmri.IsEmriNo,
            isEmri.DukkanId,
            isEmri.MusteriId,
            isEmri.Musteri.AdSoyad ?? isEmri.Musteri.FirmaAdi ?? "",
            isEmri.Musteri.Telefon,
            isEmri.CihazId,
            $"{isEmri.Cihaz.CihazTanim.Marka} {isEmri.Cihaz.CihazTanim.Model}",
            isEmri.Cihaz.SeriNo,
            isEmri.DurumId,
            isEmri.Durum.Ad,
            isEmri.Durum.Renk,
            isEmri.ArizaAciklamasi,
            isEmri.Oncelik,
            isEmri.TahminiUcret,
            isEmri.OlusturmaTarihi
        );

        return Result<IsEmriResponse>.Succeed(response, "İş emri başarıyla güncellendi");
    }
}