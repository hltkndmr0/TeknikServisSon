using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknikServis.Application.Features.Musteriler.Commands;
using TeknikServis.Application.Features.Musteriler.Queries;
using TeknikServis.Domain.Requests.Musteri;

namespace TeknikServis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MusterilerController : ControllerBase
{
    private readonly IMediator _mediator;

    public MusterilerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Dükkanın müşterilerini getir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetMusterilerByDukkanRequest request)
    {
        var result = await _mediator.Send(new GetMusterilerByDukkanQuery(request));
        return Ok(result);
    }

    /// <summary>
    /// Müşteri ara
    /// </summary>
    [HttpGet("ara")]
    public async Task<IActionResult> Search([FromQuery] SearchMusteriRequest request)
    {
        var result = await _mediator.Send(new SearchMusteriQuery(request));
        return Ok(result);
    }

    /// <summary>
    /// Müşteri detayı
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetMusteriByIdQuery(id));

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Yeni müşteri oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMusteriRequest request)
    {
        var result = await _mediator.Send(new CreateMusteriCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Müşteri güncelle
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateMusteriRequest request)
    {
        var result = await _mediator.Send(new UpdateMusteriCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Müşteri sil
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteMusteriCommand(id));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}