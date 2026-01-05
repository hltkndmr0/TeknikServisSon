using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknikServis.Application.Features.Auth.Commands;
using TeknikServis.Application.Features.Auth.Queries;
using TeknikServis.Domain.Requests.Auth;

namespace TeknikServis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new LoginCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    
    [HttpPost("admin/login")]
    public async Task<IActionResult> SuperAdminLogin([FromBody] SuperAdminLoginRequest request)
    {
        var result = await _mediator.Send(new SuperAdminLoginCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    
    [HttpPost("kayit-talep")]
    public async Task<IActionResult> KayitTalepOlustur([FromBody] KayitTalepOlusturRequest request)
    {
        var result = await _mediator.Send(new KayitTalepOlusturCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("kayit-talep/onayla")]
    public async Task<IActionResult> KayitTalepOnayla([FromBody] KayitTalepOnaylaRequest request)
    {
        var result = await _mediator.Send(new KayitTalepOnaylaCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("kayit-talep/reddet")]
    public async Task<IActionResult> KayitTalepReddet([FromBody] KayitTalepReddetRequest request)
    {
        var result = await _mediator.Send(new KayitTalepReddetCommand(request));

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("kayit-talep/bekleyenler")]
    public async Task<IActionResult> GetBekleyenTalepler()
    {
        var result = await _mediator.Send(new GetBekleyenTaleplerQuery());
        return Ok(result);
    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("kayit-talep")]
    public async Task<IActionResult> GetTumTalepler([FromQuery] GetTumTaleplerRequest request)
    {
        var result = await _mediator.Send(new GetTumTaleplerQuery(request));
        return Ok(result);
    }
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpGet("kayit-talep/{id}")]
    public async Task<IActionResult> GetKayitTalepById(Guid id)
    {
        var result = await _mediator.Send(new GetKayitTalepByIdQuery(id));

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
    
    /// <summary>
    /// Test - Şifre hash oluştur (SONRA SİL)
    /// </summary>
    [HttpGet("hash/{sifre}")]
    public IActionResult GenerateHash(string sifre)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(sifre);
        return Ok(new { sifre, hash });
    }
}