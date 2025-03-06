using CloudinaryDotNet.Core;
using MercerStore.Data;
using MercerStore.Dtos.ProductDto;
using MercerStore.Interfaces;
using MercerStore.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Repositories
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

        public async Task<Category> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
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
        public async Task<PriceRangeDto> GetCategoryPriceRangeAsync(int categoryId)
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(c => c.CategoryId == categoryId)
                .Include(c => c.ProductPricing)
                .ToListAsync();

            var priceRange = new PriceRangeDto
            {
                MaxPrice =  products.Max(p => p.ProductPricing.DiscountedPrice ?? p.ProductPricing.OriginalPrice),
                MinPrice =  products.Min(p => p.ProductPricing.DiscountedPrice ?? p.ProductPricing.OriginalPrice)
            };

            return priceRange;
        }
    }
}