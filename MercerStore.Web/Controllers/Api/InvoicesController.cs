using MediatR;
using MercerStore.Web.Application.Handlers.Invoices.Queries;
using MercerStore.Web.Application.Requests.Invoices;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Api;

[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
[Route("api/invoices")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvoicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("invoices")]
    public async Task<IActionResult> GetFilteredinvoices([FromQuery] InvoiceFilterRequest request)
    {
        var result = await _mediator.Send(new GetFilteredInvoicesQuery(request));
        return Ok(result);
    }
}