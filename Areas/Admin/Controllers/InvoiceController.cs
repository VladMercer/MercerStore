using MercerStore.Areas.Admin.ViewModels;
using MercerStore.Data.Enum.Invoice;
using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models.Invoice;
using MercerStore.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class InvoiceController : Controller
    {
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly ILogService _logService;
        private readonly IRequestContextService _requestContextService;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductRepository _productRepository;

        public InvoiceController(IUserIdentifierService userIdentifierService, IInvoiceRepository invoiceRepository, IRequestContextService requestContextService, IProductRepository productRepository)
        {
            _userIdentifierService = userIdentifierService;
            _invoiceRepository = invoiceRepository;
            _requestContextService = requestContextService;
            _productRepository = productRepository;
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
            var managerId = _userIdentifierService.GetCurrentIdentifier();

            var invoice = await _invoiceRepository.GetInvoiceByManagerId(managerId);


            if (invoice == null)
            {
                invoice = new Invoice
                {
                    ManagerId = managerId,
                    SupplierId = supplierId,
                    Status = InvoiceStatus.activ,
                };
                await _invoiceRepository.AddInvoice(invoice);
            }
            var availableProducts = await _productRepository.GetIsUnassignedProducts();

            var createInvoiceViewModel = new CreateInvoiceViewModel
            {
                InvoiceId = invoice.Id,
                AvailableProducts = availableProducts.Select(a => new InvoiceProductSelectionViewModel
                {
                    ProductId = a.Id,
                    ProductName = a.Name,
                    Quantity = 1,
                    PurchasePrice = 0
                }).ToList()
            };
            return View(createInvoiceViewModel);
        }

        [HttpPost("[area]/[controller]/addItem")]
        [LogUserAction("Manager add item in invoice", "invoiceItem")]
        public async Task<IActionResult> AddItem(CreateInvoiceViewModel createInvoiceViewModel)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(createInvoiceViewModel.InvoiceId);

            if (invoice == null || invoice.Status == InvoiceStatus.Closed)
            {
                return NotFound();
            }

            Product? product = null;

            if (createInvoiceViewModel.ProductId.HasValue)
            {
                product = await _productRepository.GetProductByIdAsync(createInvoiceViewModel.ProductId);
            }

            else if (!string.IsNullOrEmpty(createInvoiceViewModel.Sku))
            {
                product = await _productRepository.GetProductBySku(createInvoiceViewModel.Sku);

            }

            if (product == null)
            {
                ModelState.AddModelError("", "Товар не найден");
                return RedirectToAction("create-invoice", new { supplierId = invoice.SupplierId });
            }

            var newInvoiceItem = new InvoiceItem
            {
                InvoiceId = invoice.Id,
                ProductId = product.Id,
                Quantity = createInvoiceViewModel.Quantity,
                PurchasePrice = createInvoiceViewModel.ProductPrice,

            };

            await _invoiceRepository.AddInvoiceItems(newInvoiceItem);

            var selectedProducts = createInvoiceViewModel.AvailableProducts
           .Where(p => p.IsSelected)
           .ToList();


            foreach (var selectedProduct in selectedProducts)
            {
                var invoiceItem = new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    ProductId = selectedProduct.ProductId,
                    Quantity = selectedProduct.Quantity,
                    PurchasePrice = selectedProduct.PurchasePrice
                };

                await _invoiceRepository.AddInvoiceItems(invoiceItem);
            }

            var logDetails = new
            {
                createInvoiceViewModel.InvoiceId,
                ProductId = createInvoiceViewModel.ProductId ?? product.Id,
                createInvoiceViewModel.Quantity,
                PurchasePrice = createInvoiceViewModel.ProductPrice,
                createInvoiceViewModel.AvailableProducts,
                createInvoiceViewModel.Notes

            };

            _requestContextService.SetLogDetails(logDetails);

            return RedirectToAction("create-invoice", new
            {
                id = product.Id,
                supplierId = invoice.SupplierId
            });
        }

        [HttpPost("[area]/[controller]/close-invoice")]
        [LogUserAction("Manager close invoice", "invoice")]
        public async Task<IActionResult> CloseInvoice(int invoiceId, string notes)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);

            if (invoice == null || invoice.Status == InvoiceStatus.Closed)
            {
                return NotFound();
            }

            invoice.Notes = notes;

            foreach (var item in invoice.InvoiceItems)
            {
                await _productRepository.DecreaseProductStock(item.ProductId, item.Product.SKU, item.Quantity);
            }

            invoice.Status = InvoiceStatus.Received;

            invoice.TotalAmount = invoice.InvoiceItems.Sum(item => item.PurchasePrice * item.Quantity);
            await _invoiceRepository.UpdateInvoice(invoice);

            var logDetails = new
            {
                invoice.Id,
                invoice.ManagerId,
                invoice.SupplierId,
                invoice.TotalAmount,
                invoice.Notes
            };

            _requestContextService.SetLogDetails(logDetails);
            return RedirectToAction("InvoicePage", new { id = invoice.Id });
        }

        [HttpGet("[area]/[controller]/update/{invoiceId}")]
        public async Task<IActionResult> UpdateInvoice(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);
            var updateInvoiceViewModel = new UpdateInvoiceViewModel
            {
                Id = invoice.Id,
                DateReceived = invoice.DateReceived,
                EditDate = invoice.EditDate,
                PaymentDate = invoice.PaymentDate,
                SupplierId = invoice.SupplierId,
                CompanyName = invoice.Supplier.Name,
                Status = invoice.Status,
                Notes = invoice.Notes,
                TotalAmount = invoice.TotalAmount,
                InvoiceItems = invoice.InvoiceItems.Select(i => new InvoiceItemViewModel
                {
                    Id = i.Id,
                    ImageUrl = i.Product.MainImageUrl,
                    ProductId = i.Product.Id,
                    ProductName = i.Product.Name,
                    PurchasePrice = i.PurchasePrice,
                    Quantity = i.Quantity
                }).ToList()
            };

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

            var updatedTotalAmount = updateInvoiceViewModel.InvoiceItems.Sum(i => i.Quantity * i.PurchasePrice);

            if (updateInvoiceViewModel.Status == InvoiceStatus.Closed)
            {
                updateInvoiceViewModel.PaymentDate = DateTime.UtcNow;
            }

            updateInvoiceViewModel.EditDate = DateTime.UtcNow;

            var invoice = await _invoiceRepository.GetInvoiceById(updateInvoiceViewModel.Id);

            invoice.EditDate = updateInvoiceViewModel.EditDate;
            invoice.PaymentDate = updateInvoiceViewModel.PaymentDate;
            invoice.Status = updateInvoiceViewModel.Status;
            invoice.Notes = updateInvoiceViewModel.Notes;
            invoice.TotalAmount = updatedTotalAmount;

            await _invoiceRepository.UpdateInvoice(invoice);

            var updateInvoiceItems = updateInvoiceViewModel.InvoiceItems.Select(i => new InvoiceItemViewModel
            {
                Id = i.Id,
                Quantity = i.Quantity,
                PurchasePrice = i.PurchasePrice
            }).ToList();

            await _invoiceRepository.UpdateInvoiceItems(updateInvoiceItems);

            var logDetails = new
            {
                updateInvoiceViewModel.Status,
                invoice.ManagerId,
                invoice.SupplierId
            };

            _requestContextService.SetLogDetails(logDetails);

            return RedirectToAction("InvoicePage", new { id = invoice.Id });
        }
    }
}
