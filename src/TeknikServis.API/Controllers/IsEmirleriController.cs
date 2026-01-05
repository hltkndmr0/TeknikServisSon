using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknikServis.Application.Features.IsEmirleri.Commands;
using TeknikServis.Application.Features.IsEmirleri.Queries;
using TeknikServis.Domain.Requests.IsEmri;

namespace TeknikServis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IsEmirleriController : ControllerBase
{
    private readonly IMediator _mediator;

    public IsEmirleriController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// İş durumlarını getir
    /// </summary>
    [HttpGet("durumlar")]
    public async Task<IActionResult> GetDurumlar()
    {
        var result = await _mediator.Send(new GetIsDurumlariQuery());
        return Ok(result);
    }

    /// <summary>
    /// Dükkanın iş emirlerini getir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetIsEmirleriByDukkanRequest request)
    {
        var result = await _mediator.Send(new GetIsEmirleriByDukkanQuery(request));
        return Ok(result);
    }

    /// <summary>
    /// Duruma göre iş emirlerini getir
    /// </summary>
    [HttpGet("durum/{durumId}")]
    public async Task<IActionResult> GetByDurum(Guid durumId)
    {
        var result = await _mediator.Send(new GetIsEmirleriByDurumQuery(durumId));
        return Ok(result);
    }

    /// <summary>
    /// Müşterinin iş emirlerini getir
    /// </summary>
    [HttpGet("musteri/{musteriId}")]
    public async Task<IActionResult> GetByMusteri(Guid musteriId)
    {
        var result = await _mediator.Send(new GetIsEmirleriByMusteriQuery(musteriId));
        return Ok(result);
    }

    /// <summary>
    /// İş emri detayı
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetIsEmriByIdQuery(id));

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Yeni iş emri oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIsEmriRequest request)
    {
        var result = await _mediator.Send(new CreateIsEmriCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// İş emri güncelle
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateIsEmriRequest request)
    {
        var result = await _mediator.Send(new UpdateIsEmriCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// İş emri durumunu güncelle
    /// </summary>
    [HttpPatch("durum")]
    public async Task<IActionResult> UpdateDurum([FromBody] UpdateIsEmriDurumRequest request)
    {
        var result = await _mediator.Send(new UpdateIsEmriDurumCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// İş emrine not ekle
    /// </summary>
    [HttpPost("not")]
    public async Task<IActionResult> AddNot([FromBody] AddIsNotuRequest request)
    {
        var result = await _mediator.Send(new AddIsNotuCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}