using MediatR;
using MercerStore.Web.Application.Handlers.Suppliers.Commands;
using MercerStore.Web.Application.Handlers.Suppliers.Queries;
using MercerStore.Web.Areas.Admin.ViewModels.Suppliers;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
public class SupplierController : Controller
{
    private readonly IMediator _mediator;

    public SupplierController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("[area]/[controller]/suppliers")]
    public IActionResult SupplierPage()
    {
        return View();
    }

    [HttpGet("[area]/[controller]/create")]
    public IActionResult CreateSupplier()
    {
        return View();
    }

    [HttpPost("[area]/[controller]/create")]
    public async Task<IActionResult> CreateSupplier(CreateSupplierViewModel createSupplierViewModel)
    {
        if (!ModelState.IsValid) return View(createSupplierViewModel);

        await _mediator.Send(new CreateSupplierCommand(createSupplierViewModel));

        return RedirectToAction("CreateSupplier");
    }

    [HttpGet("[area]/[controller]/update/{supplierId}")]
    public async Task<IActionResult> UpdateSupplier(int supplierId)
    {
        var updateSupplierViewModel = await _mediator.Send(new GetUpdateSupplierViewModelQuery(supplierId));
        return View(updateSupplierViewModel);
    }

    [HttpPost("[area]/[controller]/update/{supplierId}")]
    public async Task<IActionResult> UpdateSupplier(UpdateSupplierViewModel updateSupplierViewModel)
    {
        if (!ModelState.IsValid) return View(updateSupplierViewModel);

        var result = await _mediator.Send(new UpdateSupplierCommand(updateSupplierViewModel));

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.ErrorMessage);
            return BadRequest();
        }

        return RedirectToAction("UpdateSupplier");
    }
}