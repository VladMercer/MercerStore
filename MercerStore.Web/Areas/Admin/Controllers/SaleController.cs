using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Sales;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class SaleController : Controller
    {
       private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet("[area]/[controller]/create-offline-sale")]
        public async Task<IActionResult> CreateOfflineSale()
        {
            var sale = await _saleService.CreateOfflineSale();
            return View(sale);
        }

        [HttpPost("[area]/[controller]/addItem")]
        [LogUserAction("Manager add item in ofline sale", "OfflineSaleItem")]
        public async Task<IActionResult> AddItem(SaleRequest request)
        {
            var result = await _saleService.AddItem(request);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return NotFound();
            }

            return RedirectToAction("CreateOfflineSale", new { id = request.SaleId });
        }

        [HttpPost("[area]/[controller]/closeSale")]
        [LogUserAction("Manager close offline sale", "OfflineSale")]
        public async Task<IActionResult> CloseSale(int saleId)
        {
            var result = await _saleService.CloseSale(saleId);
            return RedirectToAction("SaleSummary", new { id = result.Data });
        }

        [HttpGet("[area]/[controller]/summary/{id}")]
        public async Task<IActionResult> SaleSummary(int id)
        {
            var result = await _saleService.GetSummarySale(id);
            return View(result.Data);
        }
    }
}
