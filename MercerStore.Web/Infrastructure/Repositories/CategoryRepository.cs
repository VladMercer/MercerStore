using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<int> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(int categoryId)
        {
            await _context.Categories.FindAsync(categoryId);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(c => c.CategoryId == categoryId)
                .Include(c => c.ProductPricing)
                .ToListAsync();
        }
    }
}