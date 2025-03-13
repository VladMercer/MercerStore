using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<int> AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int categoryId);
        Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId);


    }
}