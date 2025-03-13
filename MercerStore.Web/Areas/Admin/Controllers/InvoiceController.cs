using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MercerStore.Web.Infrastructure.Data.Enum.Invoice;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
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
            var createInvoiceViewModel = await _invoiceService.AddInvoice(supplierId);
            return View(createInvoiceViewModel);
        }

        [HttpPost("[area]/[controller]/addItem")]
        [LogUserAction("Manager add item in invoice", "invoiceItem")]
        public async Task<IActionResult> AddItem(CreateInvoiceViewModel createInvoiceViewModel)
        {
            var result = await _invoiceService.AddInvoiceItem(createInvoiceViewModel);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);

                if (result.ErrorMessage == "Продукт не найден")
                {
                    return RedirectToAction("create-invoice", new { supplierId = createInvoiceViewModel.SupplierId });
                }

                return View(createInvoiceViewModel);
            }

            return RedirectToAction("create-invoice", new
            {
                id = result.Data.ProductId,
                supplierId = result.Data.SupplierId
            });
        }

        [HttpPost("[area]/[controller]/close-invoice")]
        [LogUserAction("Manager close invoice", "invoice")]
        public async Task<IActionResult> CloseInvoice(int invoiceId, string notes)
        {
            var result = await _invoiceService.CloseInvoice(invoiceId, notes);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
            }
         
            return RedirectToAction("InvoicePage", new { id = result.Data });
        }

        [HttpGet("[area]/[controller]/update/{invoiceId}")]
        public async Task<IActionResult> UpdateInvoice(int invoiceId)
        {
            var updateInvoiceViewModel = await _invoiceService.GetUpdateInvoiceViewModel(invoiceId);
            return View(updateInvoiceViewModel);
        }

        [HttpPost("[area]/[controller]/update/{invoiceId}")]
        [LogUserAction("Manager update invoice", "invoice")]
        public async Task<IActionResult> UpdateInvoice(UpdateInvoiceViewModel updateInvoiceViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateInvoiceViewModel);
            }

            var invoiceId = await _invoiceService.UpdateInvoice(updateInvoiceViewModel);

            return RedirectToAction("InvoicePage", new { id = invoiceId });
        }
    }
}
