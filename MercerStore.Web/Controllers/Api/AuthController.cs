using MediatR;
using MercerStore.Web.Application.Handlers.Auth.Commands;
using MercerStore.Web.Application.Requests.Account;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("generate-token")]
    public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest request)
    {
        var token = await _mediator.Send(new GenerateTokenCommand(request));
        Response.Cookies.Append("OhCookies", token);

        return Ok(new { Token = token });
    }
}