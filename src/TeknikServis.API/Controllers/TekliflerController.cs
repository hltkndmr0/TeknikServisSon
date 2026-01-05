using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknikServis.Application.Features.Teklifler.Commands;
using TeknikServis.Application.Features.Teklifler.Queries;
using TeknikServis.Domain.Requests.Teklif;

namespace TeknikServis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TekliflerController : ControllerBase
{
    private readonly IMediator _mediator;

    public TekliflerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Teklif detayı
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTeklifByIdQuery(id));

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// İş emrine ait teklifleri getir
    /// </summary>
    [HttpGet("isemri/{isEmriId}")]
    public async Task<IActionResult> GetByIsEmri(Guid isEmriId)
    {
        var result = await _mediator.Send(new GetTekliflerByIsEmriQuery(isEmriId));
        return Ok(result);
    }

    /// <summary>
    /// Teklif PDF indir
    /// </summary>
    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> GetPdf(Guid id)
    {
        var result = await _mediator.Send(new GetTeklifPdfQuery(id));

        if (!result.Success)
            return NotFound(result);

        return File(result.Data!, "application/pdf", $"Teklif_{id}.pdf");
    }

    /// <summary>
    /// Yeni teklif oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeklifRequest request)
    {
        var result = await _mediator.Send(new CreateTeklifCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Teklifi onayla
    /// </summary>
    [HttpPost("onayla")]
    public async Task<IActionResult> Onayla([FromBody] OnaylaTeklifRequest request)
    {
        var result = await _mediator.Send(new OnaylaTeklifCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Teklifi reddet
    /// </summary>
    [HttpPost("reddet")]
    public async Task<IActionResult> Reddet([FromBody] ReddetTeklifRequest request)
    {
        var result = await _mediator.Send(new ReddetTeklifCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}