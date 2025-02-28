using MercerStore.Areas.Admin.ViewModels;
using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class SupplierController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ILogService _logService;
        private readonly IRequestContextService _requestContextService;
        public SupplierController(ISupplierRepository supplierRepository, ILogService logService, IRequestContextService requestContextService)
        {
            _supplierRepository = supplierRepository;
            _logService = logService;
            _requestContextService = requestContextService;
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

            var supplier = new Supplier
            {
                Name = createSupplierViewModel.Name,
                Address = createSupplierViewModel.Address,
                Phone = createSupplierViewModel.Phone,
                ContactPerson = createSupplierViewModel.ContactPerson,
                Email = createSupplierViewModel.Email,
                IsCompany = createSupplierViewModel.IsCompany,
                TaxId = createSupplierViewModel.TaxId
            };

            var newSupplier = await _supplierRepository.AddSupplier(supplier);

            var logDetails = new
            {
                newSupplier.Id,
                createSupplierViewModel.Name,
                createSupplierViewModel.Address,
                createSupplierViewModel.Phone,
                createSupplierViewModel.ContactPerson,
                createSupplierViewModel.Email,
                createSupplierViewModel.IsCompany,
                createSupplierViewModel.TaxId

            };

            _requestContextService.SetLogDetails(logDetails);

            return RedirectToAction("CreateSupplier", new { id = newSupplier.Id });
        }

        [HttpGet("[area]/[controller]/update/{supplierId}")]
        public async Task<IActionResult> UpdateSupplier(int supplierId)
        {
            var supplier = await _supplierRepository.GetSupplierById(supplierId);

            var viewModel = new UpdateSupplierViewModel
            {
                Id = supplier.Id,
                Address = supplier.Address,
                Phone = supplier.Phone,
                Name = supplier.Name,
                ContactPerson = supplier.ContactPerson,
                Email = supplier.Email,
                IsCompany = supplier.IsCompany,
                TaxId = supplier.TaxId
            };

            return View(viewModel);
        }

        [HttpPost("[area]/[controller]/update/{supplierId}")]
        [LogUserAction("Manager update supplier", "supplier")]
        public async Task<IActionResult> UpdateSupplier(UpdateSupplierViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var supplier = await _supplierRepository.GetSupplierById(viewModel.Id);
            if (supplier == null)
            {
                return NotFound();
            }

            supplier.Address = viewModel.Address;
            supplier.Name = viewModel.Name;
            supplier.Phone = viewModel.Phone;
            supplier.Email = viewModel.Email;
            supplier.ContactPerson = viewModel.ContactPerson;
            supplier.IsCompany = viewModel.IsCompany;
            supplier.TaxId = viewModel.TaxId;

            var logDetails = new
            {
                supplier.Id,
                supplier.Address,
                supplier.Name,
                supplier.Phone,
                supplier.Email,
                supplier.ContactPerson,
                supplier.IsCompany,
                supplier.TaxId
            };

            _requestContextService.SetLogDetails(logDetails);

            await _supplierRepository.UpdateSupplier(supplier);

            return RedirectToAction("UpdateSupplier", new { id = supplier.Id });
        }
    }
}

