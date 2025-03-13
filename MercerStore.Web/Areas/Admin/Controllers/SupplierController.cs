using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Suppliers;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class SupplierController : Controller
    {

        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
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
        [LogUserAction("Manager create new supplier", "Supplier")]
        public async Task<IActionResult> CreateSupplier(CreateSupplierViewModel createSupplierViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createSupplierViewModel);
            }
            var supplierId = await _supplierService.CreateSupplier(createSupplierViewModel);

            return RedirectToAction("CreateSupplier", new { id = supplierId });
        }

        [HttpGet("[area]/[controller]/update/{supplierId}")]
        public async Task<IActionResult> UpdateSupplier(int supplierId)
        {
            var updateSupplierViewModel = await _supplierService.GetUpdateSupplierViewModel(supplierId);
            return View(updateSupplierViewModel);
        }

        [HttpPost("[area]/[controller]/update/{supplierId}")]
        [LogUserAction("Manager update supplier", "supplier")]
        public async Task<IActionResult> UpdateSupplier(UpdateSupplierViewModel updateSupplierViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateSupplierViewModel);
            }

            var result = await _supplierService.UpdateSupplier(updateSupplierViewModel);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return BadRequest();
            }

            return RedirectToAction("UpdateSupplier", new { id = result.Data });
        }
    }
}

