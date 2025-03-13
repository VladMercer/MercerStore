using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Products;
using MercerStore.Web.Infrastructure.Data.Enum.Product;

namespace MercerStore.Web.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product> Products, int TotalItems)> GetProductsAsync(ProductFilterRequest request);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int? categoryId);
        Task<Product> GetProductByIdAsync(int? productId);
        Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> productIds);
        Task<IEnumerable<Product>> GetLastProductsAsync(int count);
        Task<int?> GetCategoryByProductId(int productId);
        Task<IEnumerable<Product>> GetRandomProductsAsync(int count);
        Task<IEnumerable<Review>> GetAllReview(int productId);
        Task<IEnumerable<Review>> GetAllReviewByUser(string userId);
        Task<Product> AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int productId);
        Task DecreaseProductStock(int? productId, string? sku, int quantity);
        Task<Product> GetProductBySku(string? sku);
        Task<List<Product?>> GetIsUnassignedProducts();
    }
}