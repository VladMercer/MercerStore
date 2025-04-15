using MediatR;
using MercerStore.Web.Application.Handlers.Suppliers.Commands;
using MercerStore.Web.Application.Handlers.Suppliers.Queries;
using MercerStore.Web.Application.Requests.Suppliers;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api;

[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
[Route("api/suppliers")]
[ApiController]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("suppliers")]
    public async Task<IActionResult> GetFilteredSuppliers([FromQuery] SupplierFilterRequest request)
    {
        var result = await _mediator.Send(new GetFilteredSuppliersQuery(request));
        return Ok(result);
    }

    [HttpDelete("supplier/{supplierId}")]
    public async Task<IActionResult> RemoveSupplier(int supplierId)
    {
        await _mediator.Send(new RemoveSupplierCommand(supplierId));
        return Ok();
    }
}