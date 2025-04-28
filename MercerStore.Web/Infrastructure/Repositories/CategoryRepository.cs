using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken ct)
    {
        return await _context.Categories.ToListAsync(ct);
    }

    public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Categories.FindAsync(id, ct);
    }

    public async Task<Category> AddCategory(Category category, CancellationToken ct)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync(ct);
        return category;
    }

    public async Task UpdateCategory(Category category, CancellationToken ct)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteCategory(int categoryId, CancellationToken ct)
    {
        await _context.Categories.FindAsync(categoryId, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId, CancellationToken ct)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(c => c.CategoryId == categoryId)
            .Include(c => c.ProductPricing)
            .ToListAsync(ct);
    }
}