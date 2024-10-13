using MercerStore.Interfaces;
using MercerStore.Models;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Data
{
	public class ProductRepository : IProductRepository
	{
		private readonly AppDbContext _context;
		public ProductRepository(AppDbContext context)
		{
			_context = context;

		}

		public async Task<IEnumerable<Product>> GetAllProductsAsync()
		{
			return await _context.Products.Include(p => p.Category)
									.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
		{
			return await _context.Products.Where(p => p.CategoryId == categoryId)
									.Include(p => p.Category)
									.ToListAsync();
		}
		public async Task<int?> GetCategoryByProductId(int productId)
		{
			var product = await _context.Products
				.Include(p => p.Category)
				.FirstOrDefaultAsync(p => p.Id == productId);
			return product?.CategoryId;
		}
		public async Task<Product> GetProductByIdAsync(int productId)
		{
			return await _context.Products.Include(p => p.Category)
									.FirstOrDefaultAsync(p => p.Id == productId);
		}
		public async Task<IEnumerable<Product>> GetLastProductsAsync(int count)
		{
			return await _context.Products.OrderByDescending(p => p.Id).Take(count).ToListAsync();
		}
		public bool AddProduct(Product product)
		{
			_context.Products.Add(product);

			return Save();
		}
		public async Task<IEnumerable<Product>> GetRandomProductsAsync(int count)
		{
			
			var productIds = await _context.Products
				.Select(p => p.Id)
				.ToListAsync();

			if (productIds.Count <= count)
			{
				return await _context.Products.ToListAsync();
			}
			var random = new Random();
			var randomProductIds = productIds
				.OrderBy(x => random.Next())  
				.Take(count)                  
				.ToList();

			return await _context.Products
				.Where(p => randomProductIds.Contains(p.Id))
				.ToListAsync();
		}
		public bool UpdateProduct(Product product)
		{
			_context.Entry(product).State = EntityState.Modified;
			return Save();
		}

		public bool DeleteProduct(int productId)
		{
			var product = _context.Products.Find(productId);
			if (product != null)
			{
				_context.Products.Remove(product);
			}
			return Save();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		
	}
}