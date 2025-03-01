using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models.Products;
using MercerStore.Models.sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class SaleController : Controller
    {
        private readonly ILogService _logService;
        private readonly IRequestContextService _requestContextService;
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserIdentifierService _userIdentifierService;

        public SaleController(ILogService logService, IProductRepository productRepository, IUserIdentifierService userIdentifierService, IRequestContextService requestContextService, ISaleRepository saleRepository)
        {
            _logService = logService;
            _productRepository = productRepository;
            _userIdentifierService = userIdentifierService;
            _requestContextService = requestContextService;
            _saleRepository = saleRepository;
        }

        [HttpGet("[area]/[controller]/create-offline-sale")]
        public async Task<IActionResult> CreateOfflineSale()
        {
            var managerId = _userIdentifierService.GetCurrentIdentifier();

            var sale = await _saleRepository.GetSaleByManagerId(managerId);

            if (sale == null)
            {
                sale = new OfflineSale { ManagerId = managerId };
                await _saleRepository.AddOfflineSales(sale);
            }

            return View(sale);
        }

        [HttpPost("[area]/[controller]/addItem")]
        [LogUserAction("Manager add item in ofline sale", "OfflineSaleItem")]
        public async Task<IActionResult> AddItem(int saleId, int? productId, string? sku, int quantity)
        {
            var sale = await _saleRepository.GetSaleById(saleId);

            if (sale == null || sale.IsClosed)
            {
                return NotFound();
            }

            Product? product = null;

            if (productId.HasValue)
            {
                product = await _productRepository.GetProductByIdAsync(productId);
            }

            else if (!string.IsNullOrEmpty(sku))
            {
                product = await _productRepository.GetProductBySku(sku);
            }

            if (product == null)
            {
                ModelState.AddModelError("", "Товар не найден");
                return RedirectToAction("CreateOfflineSale");
            }

            var newSaleItem = new OfflineSaleItem
            {
                OfflineSaleId = saleId,
                ProductId = productId ?? product.Id,
                SKU = sku,
                Quantity = quantity,
                ItemPrice = product.ProductPricing.DiscountedPrice ?? product.ProductPricing.OriginalPrice
            };

            await _saleRepository.AddOfflineSaleItems(newSaleItem);

            var logDetails = new
            {
                OfflineSaleId = saleId,
                ProductId = productId,
                sku,
                Quantity = quantity,
                sale.ManagerId

            };

            _requestContextService.SetLogDetails(logDetails);

            return RedirectToAction("CreateOfflineSale", new { id = saleId });
        }

        [HttpPost("[area]/[controller]/closeSale")]
        [LogUserAction("Manager close offline sale", "OfflineSale")]
        public async Task<IActionResult> CloseSale(int saleId)
        {
            var sale = await _saleRepository.GetSaleById(saleId);

            if (sale == null || sale.IsClosed)
            {
                return NotFound();
            }

            foreach (var item in sale.Items)
            {
                await _productRepository.DecreaseProductStock(item.ProductId, item.SKU, item.Quantity);
            }

            sale.IsClosed = true;
            sale.TotalPrice = sale.Items.Sum(item => item.ItemPrice * item.Quantity);
            await _saleRepository.UpdateSale(sale);

            var logDetails = new
            {
                OfflineSaleId = saleId,
                sale.ManagerId,
                sale.TotalPrice
            };

            _requestContextService.SetLogDetails(logDetails);
            return RedirectToAction("SaleSummary", new { id = sale.Id });
        }

        [HttpGet("[area]/[controller]/summary/{id}")]
        public async Task<IActionResult> SaleSummary(int id)
        {
            var sale = await _saleRepository.GetSaleById(id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }
    }
}
