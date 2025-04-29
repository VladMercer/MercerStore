using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Products;
using MercerStore.Web.Application.ViewModels.Products;
using MercerStore.Web.Infrastructure.Data;
using MercerStore.Web.Infrastructure.Data.Enum.Product;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Product> Products, int TotalItems)> GetProductsAsync(ProductFilterRequest request,
        CancellationToken ct)
    {
        var productsQuery = _context.Products
            .Include(p => p.ProductDescription)
            .Include(p => p.ProductPricing)
            .Include(p => p.ProductStatus)
            .AsQueryable();


        if (request.CategoryId.HasValue) productsQuery = productsQuery.Where(p => p.CategoryId == request.CategoryId);


        productsQuery = request.Filter switch
        {
            AdminProductFilter.OutOfStock => productsQuery.Where(p =>
                p.ProductStatus.Status == ProductStatuses.OutOfStock),
            AdminProductFilter.InStock => productsQuery.Where(p => p.ProductStatus.Status == ProductStatuses.Available),
            AdminProductFilter.Archived => productsQuery.Where(p => p.ProductStatus.Status == ProductStatuses.Archived),
            _ => productsQuery
        };

        productsQuery = request.SortOrder switch
        {
            AdminProductSortOrder.NameAsc => productsQuery.OrderBy(p => p.Name),
            AdminProductSortOrder.NameDesc => productsQuery.OrderByDescending(p => p.Name),
            AdminProductSortOrder.StatusAsc => productsQuery.OrderBy(p => p.ProductStatus.Status),
            AdminProductSortOrder.StatusDesc => productsQuery.OrderByDescending(p => p.ProductStatus.Status),
            AdminProductSortOrder.InStockAsc => productsQuery.OrderBy(p => p.ProductStatus.InStock),
            AdminProductSortOrder.InStockDesc => productsQuery.OrderByDescending(p => p.ProductStatus.InStock),
            _ => productsQuery.OrderBy(p => p.Name)
        };

        var totalItems = await productsQuery.CountAsync(ct);

        var products = await productsQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        products = request.SortOrder switch
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

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken ct)
    {
        return await _context.Products.Include(p => p.Category)
            .Include(p => p.ProductDescription)
            .Include(p => p.ProductPricing)
            .Include(p => p.ProductStatus)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int? categoryId,
        CancellationToken ct)
    {
        return await _context.Products.Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .Include(p => p.ProductPricing)
            .Include(p => p.ProductDescription)
            .ToListAsync(ct);
    }

    public async Task<int?> GetCategoryByProductId(int productId, CancellationToken ct)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == productId, ct);
        return product?.CategoryId;
    }

    public async Task<Product> GetProductByIdAsync(int? productId, CancellationToken ct)
    {
        return await _context.Products
            .Include(p => p.ProductDescription)
            .Include(p => p.ProductPricing)
            .Include(p => p.ProductStatus)
            .FirstAsync(p => p.Id == productId, ct);
    }

    public async Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> productIds,
        CancellationToken ct)
    {
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .Include(p => p.ProductDescription)
            .Include(p => p.ProductPricing)
            .Include(p => p.ProductStatus)
            .ToListAsync(ct);

        var productIdsList = productIds.ToList();
        return products.OrderBy(p => productIdsList.IndexOf(p.Id));
    }

    public async Task<IEnumerable<Product>> GetLastProductsAsync(int count, CancellationToken ct)
    {
        return await _context.Products.AsNoTracking()
            .OrderByDescending(p => p.Id).Take(count)
            .Include(p => p.ProductDescription)
            .Include(p => p.ProductPricing)
            .Include(p => p.ProductStatus)
            .ToListAsync(ct);
    }

    public async Task AddProduct(Product product, CancellationToken ct)
    {
        await _context.Products.AddAsync(product, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<RandomProductViewModel>> GetRandomProductsAsync(int count,
        CancellationToken ct)
    {
        return await _context.Products
            .AsNoTracking()
            .OrderBy(x => EF.Functions.Random())
            .Select(p => new RandomProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                MainImageUrl = p.MainImageUrl,
                Description = p.ProductDescription.DescriptionText,
                Price = p.ProductPricing.OriginalPrice,
                DiscountedPrice = p.ProductPricing.DiscountedPrice
            })
            .Take(count)
            .ToListAsync(ct);
    }

    public async Task UpdateProduct(Product product, CancellationToken ct)
    {
        _context.Update(product);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteProduct(int productId, CancellationToken ct)
    {
        var product = await _context.Products.FindAsync(productId, ct);
        if (product != null) _context.Products.Remove(product);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Review>> GetAllReview(int productId, CancellationToken ct)
    {
        return await _context.Reviews.Where(p => p.ProductId == productId).Include(r => r.User)
            .ToListAsync(ct);
    }

    public Task<IEnumerable<Review>> GetAllReviewByUser(string userId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task DecreaseProductStock(int? productId, string? sku, int quantity,
        CancellationToken ct)
    {
        var product = await _context.Products.Include(p => p.ProductStatus)
            .FirstOrDefaultAsync(p => p.Id == productId || p.SKU == sku, ct);
        if (product != null)
        {
            var quantityInStock = product.ProductStatus.InStock;
            quantityInStock -= quantity;
            if (quantityInStock < 0) quantityInStock = 0;
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<Product> GetProductBySku(string? sku, CancellationToken ct)
    {
        return await _context.Products
            .Include(p => p.ProductDescription)
            .Include(p => p.ProductPricing)
            .Include(p => p.ProductStatus)
            .FirstOrDefaultAsync(p => p.SKU == sku, ct);
    }

    public async Task<IEnumerable<Product?>> GetIsUnassignedProducts(CancellationToken ct)
    {
        return await _context.Products.Include(p => p.ProductStatus).Where(p => p.ProductStatus.IsUnassigned == true)
            .ToListAsync(ct);
    }
}
