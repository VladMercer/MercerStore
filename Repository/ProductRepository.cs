using MercerStore.Data.Enum.Product;
using MercerStore.Interfaces;
using MercerStore.Models.Products;
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

        public async Task<(IEnumerable<Product> Products, int TotalItems)> GetProductsAsync(
            int? categoryId,
            AdminProductSortOrder sortOrder,
            AdminProductFilter filter,
            int pageNumber,
            int pageSize)
        {
            var productsQuery = _context.Products
                .Include(p => p.ProductDescription)
                .Include(p => p.ProductPricing)
                .Include(p => p.ProductStatus)
                .AsQueryable();


            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId);
            }


            productsQuery = filter switch
            {
                AdminProductFilter.OutOfStock => productsQuery.Where(p => p.ProductStatus.Status == ProductStatuses.OutOfStock),
                AdminProductFilter.InStock => productsQuery.Where(p => p.ProductStatus.Status == ProductStatuses.Available),
                AdminProductFilter.Archived => productsQuery.Where(p => p.ProductStatus.Status == ProductStatuses.Archived),
                _ => productsQuery
            };

            productsQuery = sortOrder switch
            {
                AdminProductSortOrder.NameAsc => productsQuery.OrderBy(p => p.Name),
                AdminProductSortOrder.NameDesc => productsQuery.OrderByDescending(p => p.Name),
                AdminProductSortOrder.StatusAsc => productsQuery.OrderBy(p => p.ProductStatus.Status),
                AdminProductSortOrder.StatusDesc => productsQuery.OrderByDescending(p => p.ProductStatus.Status),
                AdminProductSortOrder.InStockAsc => productsQuery.OrderBy(p => p.ProductStatus.InStock),
                AdminProductSortOrder.InStockDesc => productsQuery.OrderByDescending(p => p.ProductStatus.InStock),
                _ => productsQuery.OrderBy(p => p.Name)
            };

            var totalItems = await productsQuery.CountAsync();

            var products = await productsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            products = sortOrder switch
            {
                AdminProductSortOrder.PriceAsc => products.OrderBy(
                    p => p.ProductPricing.DiscountedPrice ?? p.ProductPricing.OriginalPrice)
                    .ToList(),
                AdminProductSortOrder.PriceDesc => products.OrderByDescending(
                    p => p.ProductPricing.DiscountedPrice ?? p.ProductPricing.OriginalPrice)
                    .ToList(),
                _ => products
            };
            return (products, totalItems);
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.Category)
                .Include(p => p.ProductDescription)
                .Include(p => p.ProductPricing)
                .Include(p => p.ProductStatus)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int? categoryId)
        {
            return await _context.Products.Where(p => p.CategoryId == categoryId)
                                    .Include(p => p.Category)
                                    .Include(p => p.ProductPricing)
                                    .Include(p => p.ProductDescription)
                                    .ToListAsync();
        }
        public async Task<int?> GetCategoryByProductId(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == productId);
            return product?.CategoryId;
        }
        public async Task<Product> GetProductByIdAsync(int? productId)
        {
            return await _context.Products
                .Include(p => p.ProductDescription)
                .Include(p => p.ProductPricing)
                .Include(p => p.ProductStatus)
                .FirstAsync(p => p.Id == productId);
        }
        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> productIds)
        {
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .Include(p => p.ProductDescription)
                .Include(p => p.ProductPricing)
                .Include(p => p.ProductStatus)
                .ToListAsync();

            var productIdsList = productIds.ToList();
            return products.OrderBy(p => productIdsList.IndexOf(p.Id));
        }
        public async Task<IEnumerable<Product>> GetLastProductsAsync(int count)
        {
            return await _context.Products.OrderByDescending(p => p.Id).Take(count)
            .Include(p => p.ProductDescription)
            .Include(p => p.ProductPricing)
            .Include(p => p.ProductStatus)
            .ToListAsync();
        }

        public async Task<Product> AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
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
                .Include(p => p.ProductDescription)
                .Include(p => p.ProductPricing)
                .Include(p => p.ProductStatus)
                .Where(p => randomProductIds.Contains(p.Id))
                .ToListAsync();
        }
        public async Task UpdateProduct(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Review>> GetAllReview(int productId)
        {
            return await _context.Reviews.Where(p => p.ProductId == productId).Include(r => r.User).ToListAsync();

        }

        public Task<IEnumerable<Review>> GetAllReviewByUser(string userId)
        {
            throw new NotImplementedException();
        }
        public async Task DecreaseProductStock(int? productId, string? sku, int quantity)
        {
            var product = await _context.Products.Include(p => p.ProductStatus).FirstOrDefaultAsync(p => p.Id == productId || p.SKU == sku);
            if (product != null)
            {
                var quantityInStock = product.ProductStatus.InStock;
                quantityInStock -= quantity;
                if (quantityInStock < 0) quantityInStock = 0;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Product> GetProductBySku(string? sku)
        {
            return await _context.Products
                .Include(p => p.ProductDescription)
                .Include(p => p.ProductPricing)
                .Include(p => p.ProductStatus)
                .FirstOrDefaultAsync(p => p.SKU == sku);
        }
        public async Task<List<Product?>> GetIsUnassignedProducts()
        {
            return await _context.Products.Include(p => p.ProductStatus).Where(p => p.ProductStatus.IsUnassigned == true).ToListAsync();
        }
    }
}