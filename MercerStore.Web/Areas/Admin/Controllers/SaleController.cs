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
    public async Task<IActionResult> CreateOfflineSale()
    {
        var sale = await _mediator.Send(new CreateOfflineSaleQuery());
        return View(sale);
    }

    [HttpPost("[area]/[controller]/addItem")]
    public async Task<IActionResult> AddItem(SaleRequest request)
    {
        var result = await _mediator.Send(new AddItemCommand(request));

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.ErrorMessage);
            return NotFound();
        }

        return RedirectToAction("CreateOfflineSale");
    }

    [HttpPost("[area]/[controller]/closeSale")]
    public async Task<IActionResult> CloseSale(int saleId)
    {
        var result = await _mediator.Send(new CloseSaleCommand(saleId));
        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.ErrorMessage);
            return NotFound();
        }

        return RedirectToAction("SaleSummary");
    }

    [HttpGet("[area]/[controller]/summary/{id}")]
    public async Task<IActionResult> SaleSummary(int id)
    {
        var result = await _mediator.Send(new GetSummarySaleQuery(id));
        return View(result.Data);
    }
}