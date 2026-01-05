using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknikServis.Application.Features.Faturalar.Commands;
using TeknikServis.Application.Features.Faturalar.Queries;
using TeknikServis.Domain.Requests.Fatura;

namespace TeknikServis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FaturalarController : ControllerBase
{
    private readonly IMediator _mediator;

    public FaturalarController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Fatura detayı
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetFaturaByIdQuery(id));

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// İş emrine ait faturaları getir
    /// </summary>
    [HttpGet("isemri/{isEmriId}")]
    public async Task<IActionResult> GetByIsEmri(Guid isEmriId)
    {
        var result = await _mediator.Send(new GetFaturalarByIsEmriQuery(isEmriId));
        return Ok(result);
    }

    /// <summary>
    /// Fatura PDF indir
    /// </summary>
    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> GetPdf(Guid id)
    {
        var result = await _mediator.Send(new GetFaturaPdfQuery(id));

        if (!result.Success)
            return NotFound(result);

        return File(result.Data!, "application/pdf", $"Fatura_{id}.pdf");
    }

    /// <summary>
    /// Yeni fatura oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFaturaRequest request)
    {
        var result = await _mediator.Send(new CreateFaturaCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}