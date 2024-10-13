using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("/[controller]")]
[ApiController]
public class CategoryApiController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public CategoryApiController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetFilteredProducts(int categoryId, string? sortOrder, int pageNumber, int pageSize, int? maxPrice, int? minPrice)
    {
        var products = await _productRepository.GetProductsByCategoryAsync(categoryId);

        if (minPrice.HasValue)
        {
            products = products.Where(p => p.Price >= minPrice.Value); 
        }
        if (maxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= maxPrice.Value);
        }

        switch (sortOrder)
        {
            
            case "name_desc":
                products = products.OrderByDescending(p => p.Name);
                break;
            case "price_asc":
                products = products.OrderBy(p => p.Price);
                break;
            case "price_desc":
                products = products.OrderByDescending(p => p.Price);
                break;
            default:
                products = products.OrderBy(p => p.Name);
                break;
        }

        int totalItems = products.Count();
        var pagedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        var result = new
        {
            Products = pagedProducts,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
        };

        return Ok(result);
    }
    [HttpGet("price-range")]
    public async Task<IActionResult> GetPriceRange(int categoryId)
    {
        var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
        var minPrice = products.Min(p => p.Price);
        var maxPrice = products.Max(p => p.Price);

        var result = new
        {
            MinPrice =minPrice, 
            MaxPrice = maxPrice
        };
        return Ok(result);
    }
}