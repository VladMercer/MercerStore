using MercerStore.Web.Application.Dtos.Product;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Products;
using MercerStore.Web.Application.ViewModels.Products;
using MercerStore.Web.Areas.Admin.ViewModels.Products;
using MercerStore.Web.Infrastructure.Data.Enum.Product;

namespace MercerStore.Web.Application.Services;

public class ProductService : IProductService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Product> GetProductById(int productId, CancellationToken ct)
    {
        return await _productRepository.GetProductByIdAsync(productId, ct);
    }

    public async Task<PaginatedResultDto<AdminProductDto>> GetFilteredProductsWithoutCache(ProductFilterRequest request,
        CancellationToken ct)
    {
        var (products, totalItems) = await _productRepository.GetProductsAsync(request, ct);

        var pageProducts = products
            .Select(p => new AdminProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.ProductPricing.OriginalPrice,
                MainImageUrl = p.MainImageUrl,
                CategoryId = p.CategoryId,
                Description = p.ProductDescription.DescriptionText,
                DiscountedPrice = p.ProductPricing.DiscountedPrice,
                DiscountEnd = p.ProductPricing.DiscountEnd,
                DiscountStart = p.ProductPricing.DiscountStart,
                InStock = p.ProductStatus.InStock,
                RemainingDiscountDays = p.ProductPricing.RemainingDiscountDays,
                Status = p.ProductStatus.Status switch
                {
                    ProductStatuses.Available => "В наличии",
                    ProductStatuses.OutOfStock => "Нет в наличии",
                    ProductStatuses.Archived => "Снят с продажи",
                    _ => "Неизвестный статус"
                }
            });

        return new PaginatedResultDto<AdminProductDto>(pageProducts, totalItems, request.PageSize);
    }

    public async Task<ProductViewModel> GetProductDetails(int productId, CancellationToken ct)
    {
        var product = await _productRepository.GetProductByIdAsync(productId, ct);

        var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId, ct);

        var productStatus = product.ProductStatus.Status switch
        {
            ProductStatuses.Available => "В наличии",
            ProductStatuses.OutOfStock => "Нет в наличии",
            ProductStatuses.Archived => "Снят с продажи",
            _ => "Неизвестный статус"
        };

        return new ProductViewModel
        {
            ProductId = product.Id,
            Name = product.Name,
            Price = product.ProductPricing.OriginalPrice,
            SKU = product.SKU,
            MainImageUrl = product.MainImageUrl,
            Description = product.ProductDescription.DescriptionText,
            Category = category,
            CategoryId = product.CategoryId,
            Status = productStatus,
            DiscountPrice = product.ProductPricing.DiscountedPrice
        };
    }

    public async Task<IEnumerable<Category>> GetAllCategories(CancellationToken ct)
    {
        return await _categoryRepository.GetAllCategoriesAsync(ct);
    }

    public async Task AddProduct(Product product, CancellationToken ct)
    {
        await _productRepository.AddProduct(product, ct);
    }

    public async Task<UpdateProductViewModel> GetUpdateProductViewModel(int productId, CancellationToken ct)
    {
        var product = await _productRepository.GetProductByIdAsync(productId, ct);
        return new UpdateProductViewModel
        {
            Id = product.Id,
            ProductName = product.Name,
            Price = product.ProductPricing.OriginalPrice,
            RemainingDiscountDays = product.ProductPricing.RemainingDiscountDays,
            Status = product.ProductStatus.Status,
            DiscountPercentage = product.ProductPricing.DiscountPercentage,
            DiscountEnd = product.ProductPricing.DiscountEnd,
            Description = product.ProductDescription.DescriptionText,
            InStock = product.ProductStatus.InStock,
            MainImageUrl = product.MainImageUrl,
            DiscountPrice = product.ProductPricing.FixedDiscountPrice,
            DiscountStart = product.ProductPricing.DiscountStart
        };
    }

    public async Task UpdateProduct(Product product, CancellationToken ct)
    {
        await _productRepository.UpdateProduct(product, ct);
    }

    public async Task<IEnumerable<Product>> GetAllProducts(CancellationToken ct)
    {
        return await _productRepository.GetAllProductsAsync(ct);
    }
}
