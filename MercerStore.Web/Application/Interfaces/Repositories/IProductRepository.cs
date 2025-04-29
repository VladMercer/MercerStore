using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Products;
using MercerStore.Web.Application.ViewModels.Products;

namespace MercerStore.Web.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<(IEnumerable<Product> Products, int TotalItems)> GetProductsAsync(ProductFilterRequest request,
        CancellationToken ct);

    Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken ct);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int? categoryId, CancellationToken ct);
    Task<Product> GetProductByIdAsync(int? productId, CancellationToken ct);
    Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> productIds, CancellationToken ct);
    Task<IEnumerable<Product>> GetLastProductsAsync(int count, CancellationToken ct);
    Task<int?> GetCategoryByProductId(int productId, CancellationToken ct);
    Task<IEnumerable<RandomProductViewModel>> GetRandomProductsAsync(int count, CancellationToken ct);
    Task<IEnumerable<Review>> GetAllReview(int productId, CancellationToken ct);
    Task<IEnumerable<Review>> GetAllReviewByUser(string userId, CancellationToken ct);
    Task AddProduct(Product product, CancellationToken ct);
    Task UpdateProduct(Product product, CancellationToken ct);
    Task DeleteProduct(int productId, CancellationToken ct);
    Task DecreaseProductStock(int? productId, string? sku, int quantity, CancellationToken ct);
    Task<Product> GetProductBySku(string? sku, CancellationToken ct);
    Task<IEnumerable<Product?>> GetIsUnassignedProducts(CancellationToken ct);
}
