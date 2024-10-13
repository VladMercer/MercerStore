using System.Collections.Generic;
using MercerStore.Models;

namespace MercerStore.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetLastProductsAsync(int count);
		Task<int?> GetCategoryByProductId(int productId);
        Task<IEnumerable<Product>> GetRandomProductsAsync(int count);

		bool AddProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(int productId);
        bool Save();
    }
}