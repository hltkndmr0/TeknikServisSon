using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeknikServis.Application.Common.Interfaces;
using TeknikServis.Application.Common.Models;
using TeknikServis.Domain.Entities;
using TeknikServis.Domain.Requests.IsEmri;
using TeknikServis.Domain.Responses.IsEmri;

namespace TeknikServis.Application.Features.IsEmirleri.Commands;

// Command
public record AddIsNotuCommand(AddIsNotuRequest Data) : IRequest<Result<IsNotuResponse>>;

// Validator
public class AddIsNotuValidator : AbstractValidator<AddIsNotuCommand>
{
    public AddIsNotuValidator()
    {
        RuleFor(x => x.Data.IsEmriId)
            .NotEmpty().WithMessage("İş emri ID zorunludur");

        RuleFor(x => x.Data.NotMetni)
            .NotEmpty().WithMessage("Not metni zorunludur");
    }
}

// Handler
public class AddIsNotuHandler : IRequestHandler<AddIsNotuCommand, Result<IsNotuResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AddIsNotuHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<IsNotuResponse>> Handle(AddIsNotuCommand command, CancellationToken cancellationToken)
    {
        var request = command.Data;

        if (!_currentUserService.KullaniciId.HasValue)
            return Result<IsNotuResponse>.Fail("Kullanıcı bilgisi bulunamadı");

        var isEmri = await _context.IsEmirleri
            .FirstOrDefaultAsync(x => x.Id == request.IsEmriId, cancellationToken);

        if (isEmri == null)
            return Result<IsNotuResponse>.Fail("İş emri bulunamadı");

        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(x => x.Id == _currentUserService.KullaniciId.Value, cancellationToken);

        var isNotu = new IsNotu
        {
            Id = Guid.NewGuid(),
            IsEmriId = request.IsEmriId,
            KullaniciId = _currentUserService.KullaniciId.Value,
            NotMetni = request.NotMetni
        };

        _context.IsNotlari.Add(isNotu);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new IsNotuResponse(
            isNotu.Id,
            isNotu.KullaniciId,
            kullanici?.AdSoyad ?? "",
            isNotu.NotMetni,
            isNotu.OlusturmaTarihi
        );

        return Result<IsNotuResponse>.Succeed(response, "Not başarıyla eklendi");
    }
}