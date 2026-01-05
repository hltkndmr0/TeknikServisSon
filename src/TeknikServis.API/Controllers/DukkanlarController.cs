using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknikServis.Application.Features.Dukkanlar.Commands;
using TeknikServis.Application.Features.Dukkanlar.Queries;
using TeknikServis.Domain.Requests.Dukkan;

namespace TeknikServis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin")]
public class DukkanlarController : ControllerBase
{
    private readonly IMediator _mediator;

    public DukkanlarController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDukkanlarRequest request)
    {
        var result = await _mediator.Send(new GetAllDukkanlarQuery(request));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetDukkanByIdQuery(id));

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDukkanRequest request)
    {
        var result = await _mediator.Send(new CreateDukkanCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateDukkanRequest request)
    {
        var result = await _mediator.Send(new UpdateDukkanCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteDukkanCommand(id));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}