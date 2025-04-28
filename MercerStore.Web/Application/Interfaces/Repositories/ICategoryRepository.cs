using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken ct);
    Task<Category> GetCategoryByIdAsync(int id, CancellationToken ct);
    Task<Category> AddCategory(Category category, CancellationToken ct);
    Task UpdateCategory(Category category, CancellationToken ct);
    Task DeleteCategory(int categoryId, CancellationToken ct);
    Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId, CancellationToken ct);
}
