using MediatR;
using MercerStore.Web.Application.Handlers.Heartbeat.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api;

[ApiController]
[Route("api/heartbeat")]
public class HeartbeatController : ControllerBase
{
    private readonly IMediator _mediator;

    public HeartbeatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateActivity()
    {
        await _mediator.Send(new UpdateActivityCommand());
        return Ok();
    }
}