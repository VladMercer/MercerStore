using MediatR;
using MercerStore.Web.Application.Handlers.Invoices.Command;
using MercerStore.Web.Application.Handlers.Invoices.Queries;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
public class InvoiceController : Controller
{
    private readonly IMediator _mediator;

    public InvoiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("[area]/[controller]/supplier-selection")]
    public IActionResult SupplierChoice()
    {
        return View();
    }

    public IActionResult InvoicePage()
    {
        return View();
    }

    [HttpGet("[area]/[controller]/create-invoice/{supplierId}")]
    public async Task<IActionResult> CreateInvoice(int supplierId)
    {
        var createInvoiceViewModel = await _mediator.Send(new GetCreateInvoiceViewModelQuery(supplierId));
        return View(createInvoiceViewModel);
    }

    [HttpPost("[area]/[controller]/addItem")]
    public async Task<IActionResult> AddItem(CreateInvoiceViewModel createInvoiceViewModel)
    {
        var result = await _mediator.Send(new AddInvoiceItemCommand(createInvoiceViewModel));

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.ErrorMessage);

            if (result.ErrorMessage == "Продукт не найден")
                return RedirectToAction("CreateInvoice", new { supplierId = createInvoiceViewModel.SupplierId });

            return View(createInvoiceViewModel);
        }

        return RedirectToAction("CreateInvoice", new { supplierId = createInvoiceViewModel.SupplierId });
    }

    [HttpPost("[area]/[controller]/close-invoice")]
    public async Task<IActionResult> CloseInvoice(int invoiceId, string notes)
    {
        var result = await _mediator.Send(new CloseInvoiceCommand(invoiceId, notes));

        if (!result.IsSuccess) ModelState.AddModelError("", result.ErrorMessage);

        return RedirectToAction("InvoicePage");
    }

    [HttpGet("[area]/[controller]/update/{invoiceId}")]
    public async Task<IActionResult> UpdateInvoice(int invoiceId)
    {
        var updateInvoiceViewModel = await _mediator.Send(new GetUpdateInvoiceViewModelQuery(invoiceId));
        return View(updateInvoiceViewModel);
    }

    [HttpPost("[area]/[controller]/update/{invoiceId}")]
    public async Task<IActionResult> UpdateInvoice(UpdateInvoiceViewModel updateInvoiceViewModel)
    {
        if (!ModelState.IsValid) return View(updateInvoiceViewModel);

        await _mediator.Send(new UpdateInvoiceCommand(updateInvoiceViewModel));

        return RedirectToAction("InvoicePage");
    }
}