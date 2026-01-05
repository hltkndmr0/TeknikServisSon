using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknikServis.Application.Features.Cihazlar.Commands;
using TeknikServis.Application.Features.Cihazlar.Queries;
using TeknikServis.Domain.Requests.Cihaz;

namespace TeknikServis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CihazlarController : ControllerBase
{
    private readonly IMediator _mediator;

    public CihazlarController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cihaz kategorilerini getir
    /// </summary>
    [HttpGet("kategoriler")]
    public async Task<IActionResult> GetKategoriler()
    {
        var result = await _mediator.Send(new GetCihazKategorileriQuery());
        return Ok(result);
    }

    /// <summary>
    /// Yeni kategori oluştur
    /// </summary>
    [HttpPost("kategoriler")]
    public async Task<IActionResult> CreateKategori([FromBody] CreateCihazKategoriRequest request)
    {
        var result = await _mediator.Send(new CreateCihazKategoriCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Cihaz tanımlarını getir
    /// </summary>
    [HttpGet("tanimlar")]
    public async Task<IActionResult> GetTanimlar([FromQuery] GetCihazTanimlariRequest request)
    {
        var result = await _mediator.Send(new GetCihazTanimlariQuery(request));
        return Ok(result);
    }

    /// <summary>
    /// Yeni cihaz tanımı oluştur
    /// </summary>
    [HttpPost("tanimlar")]
    public async Task<IActionResult> CreateTanim([FromBody] CreateCihazTanimRequest request)
    {
        var result = await _mediator.Send(new CreateCihazTanimCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Cihaz detayı
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetCihazByIdQuery(id));

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Müşterinin cihazlarını getir
    /// </summary>
    [HttpGet("musteri/{musteriId}")]
    public async Task<IActionResult> GetByMusteri(Guid musteriId)
    {
        var result = await _mediator.Send(new GetCihazlarByMusteriQuery(musteriId));
        return Ok(result);
    }

    /// <summary>
    /// Seri numarasına göre cihaz ara
    /// </summary>
    [HttpGet("seri/{seriNo}")]
    public async Task<IActionResult> SearchBySeriNo(string seriNo)
    {
        var result = await _mediator.Send(new SearchCihazBySeriNoQuery(seriNo));

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Yeni cihaz oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCihazRequest request)
    {
        var result = await _mediator.Send(new CreateCihazCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Cihaz güncelle
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCihazRequest request)
    {
        var result = await _mediator.Send(new UpdateCihazCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetCihazlarRequest request)
    {
        var result = await _mediator.Send(new GetCihazlarQuery(request));
        return Ok(result);
    }
}