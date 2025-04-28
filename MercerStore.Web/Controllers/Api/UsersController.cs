using MediatR;
using MercerStore.Web.Application.Handlers.Users.Queries;
using MercerStore.Web.Application.Requests.Users;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api;

[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("userId")]
    public async Task<IActionResult> GetCurrentUserId()
    {
        var userId = _mediator.Send(new GetCurrentIdentifierQuery());
        return Ok(userId);
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetCurrentUserRoles()
    {
        var roles = _mediator.Send(new GetCurrentUserRolesQuery());
        return Ok(roles);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetFilteredUsers([FromQuery] UserFilterRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetFilteredUsersQuery(request), ct);
        return Ok(result);
    }
}