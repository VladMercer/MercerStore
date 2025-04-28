using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Models.sales;
using MercerStore.Web.Application.Requests.Sales;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Services;

public class SaleService : ISaleService
{
    private readonly IProductRepository _productRepository;

    private readonly ISaleRepository _saleRepository;

    public SaleService(ISaleRepository saleRepository, IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<OfflineSale> CreateOfflineSale(string managerId, CancellationToken ct)
    {
        var sale = await _saleRepository.GetSaleByManagerId(managerId, ct);

        if (sale == null)
        {
            sale = new OfflineSale { ManagerId = managerId };
            await _saleRepository.AddOfflineSales(sale, ct);
        }

        return sale;
    }

    public async Task<Result<int>> AddItem(SaleRequest request, CancellationToken ct)
    {
        var sale = await _saleRepository.GetSaleById(request.SaleId, ct);

        if (sale == null || sale.IsClosed) return Result<int>.Failure("Продажа не найдена");

        Product? product = null;

        if (request.ProductId.HasValue)
            product = await _productRepository.GetProductByIdAsync(request.ProductId, ct);

        else if (!string.IsNullOrEmpty(request.Sku))
            product = await _productRepository.GetProductBySku(request.Sku, ct);

        if (product == null) return Result<int>.Failure("Товар не найден");

        var newSaleItem = new OfflineSaleItem
        {
            OfflineSaleId = request.SaleId,
            ProductId = request.ProductId ?? product.Id,
            SKU = request.Sku,
            Quantity = request.Quantity,
            ItemPrice = product.ProductPricing.DiscountedPrice ?? product.ProductPricing.OriginalPrice
        };

        await _saleRepository.AddOfflineSaleItems(newSaleItem, ct);

        return Result<int>.Success(sale.Id);
    }

    public async Task<Result<int>> CloseSale(int saleId, CancellationToken ct)
    {
        var sale = await _saleRepository.GetSaleById(saleId, ct);

        if (sale == null || sale.IsClosed) return Result<int>.Failure("Продажа не найдена");

        foreach (var item in sale.Items)
            await _productRepository.DecreaseProductStock(item.ProductId, item.SKU, item.Quantity, ct);

        sale.IsClosed = true;
        sale.TotalPrice = sale.Items.Sum(item => item.ItemPrice * item.Quantity);
        await _saleRepository.UpdateSale(sale, ct);

        return Result<int>.Success(saleId);
    }

    public async Task<Result<OfflineSale>> GetSummarySale(int saleId, CancellationToken ct)
    {
        var sale = await _saleRepository.GetSaleById(saleId, ct);

        return sale == null ? Result<OfflineSale>.Failure("Продажа не найдена") : Result<OfflineSale>.Success(sale);
    }
}