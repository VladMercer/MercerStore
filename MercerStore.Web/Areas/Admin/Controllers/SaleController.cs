using MediatR;
using MercerStore.Web.Application.Handlers.Sales.Commands;
using MercerStore.Web.Application.Handlers.Sales.Queries;
using MercerStore.Web.Application.Requests.Sales;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
public class SaleController : Controller
{
    private readonly IMediator _mediator;

    public SaleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("[area]/[controller]/create-offline-sale")]
    public async Task<IActionResult> CreateOfflineSale(CancellationToken ct)
    {
        var sale = await _mediator.Send(new CreateOfflineSaleQuery(), ct);
        return View(sale);
    }

    [HttpPost("[area]/[controller]/addItem")]
    public async Task<IActionResult> AddItem(SaleRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new AddItemCommand(request), ct);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.ErrorMessage);
            return NotFound();
        }

        return RedirectToAction("CreateOfflineSale");
    }

    [HttpPost("[area]/[controller]/closeSale")]
    public async Task<IActionResult> CloseSale(int saleId, CancellationToken ct)
    {
        var result = await _mediator.Send(new CloseSaleCommand(saleId), ct);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.ErrorMessage);
            return NotFound();
        }

        return RedirectToAction("SaleSummary");
    }

    [HttpGet("[area]/[controller]/summary/{id}")]
    public async Task<IActionResult> SaleSummary(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetSummarySaleQuery(id), ct);
        return View(result.Data);
    }
}