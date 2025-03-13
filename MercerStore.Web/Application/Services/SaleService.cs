using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.sales;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Sales;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ILogService _logService;
        private readonly IRequestContextService _requestContextService;
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserIdentifierService _userIdentifierService;

        public SaleService(
            ILogService logService,
            IRequestContextService requestContextService,
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            IUserIdentifierService userIdentifierService)
        {
            _logService = logService;
            _requestContextService = requestContextService;
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _userIdentifierService = userIdentifierService;
        }
        public async Task<OfflineSale> CreateOfflineSale()
        {
            var managerId = _userIdentifierService.GetCurrentIdentifier();

            var sale = await _saleRepository.GetSaleByManagerId(managerId);

            if (sale == null)
            {
                sale = new OfflineSale { ManagerId = managerId };
                await _saleRepository.AddOfflineSales(sale);
            }
            
            return sale;
        }
        public async Task<Result<int>> AddItem(SaleRequest request)
        {
            var sale = await _saleRepository.GetSaleById(request.SaleId);

            if (sale == null || sale.IsClosed)
            {
                return Result<int>.Failure("Продажа не найдена");
            }

            Product? product = null;

            if (request.ProductId.HasValue)
            {
                product = await _productRepository.GetProductByIdAsync(request.ProductId);
            }

            else if (!string.IsNullOrEmpty(request.Sku))
            {
                product = await _productRepository.GetProductBySku(request.Sku);
            }

            if (product == null)
            {
                return Result<int>.Failure("Товар не найден");
            }

            var newSaleItem = new OfflineSaleItem
            {
                OfflineSaleId = request.SaleId,
                ProductId = request.ProductId ?? product.Id,
                SKU = request.Sku,
                Quantity = request.Quantity,
                ItemPrice = product.ProductPricing.DiscountedPrice ?? product.ProductPricing.OriginalPrice
            };

            await _saleRepository.AddOfflineSaleItems(newSaleItem);

            var logDetails = new
            {
                OfflineSaleId = request.SaleId,
                request.ProductId,
                request.Sku,
                request.Quantity,
                sale.ManagerId
            };

            _requestContextService.SetLogDetails(logDetails);

            return Result<int>.Success(sale.Id);
        }
        public async Task<Result<int>> CloseSale(int saleId)
        {
            var sale = await _saleRepository.GetSaleById(saleId);

            if (sale == null || sale.IsClosed)
            {
                return Result<int>.Failure("Продажа не найдена");
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

            return Result<int>.Success(saleId);
        }
        public async Task<Result<OfflineSale>> GetSummarySale(int saleId) 
        {
            var sale = await _saleRepository.GetSaleById(saleId);

            if (sale == null)
            {
                return Result<OfflineSale>.Failure("Продажа не найдена");
            }

            return Result<OfflineSale>.Success(sale);
        }
    }
}
