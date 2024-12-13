using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/categories")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public CategoriesController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Получить фильтрованные продукты по категории.
    /// </summary>
    /// <param name="categoryId">ID категории</param>
    /// <param name="sortOrder">Сортировка: "name_desc", "price_asc", "price_desc"</param>
    /// <param name="pageNumber">Номер страницы</param>
    /// <param name="pageSize">Количество продуктов на странице</param>
    /// <param name="maxPrice">Максимальная цена</param>
    /// <param name="minPrice">Минимальная цена</param>
    /// <returns>Список продуктов</returns>

    [HttpGet("products/{categoryId}/{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetFilteredProducts(int categoryId, int pageNumber, int pageSize, string? sortOrder, int? maxPrice, int? minPrice)
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

        products = sortOrder switch
        {
            "name_desc" => products.OrderByDescending(p => p.Name),
            "price_asc" => products.OrderBy(p => p.Price),
            "price_desc" => products.OrderByDescending(p => p.Price),
            _ => products.OrderBy(p => p.Name),
        };

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
    [HttpGet("price-range/{categoryId}")]
    public async Task<IActionResult> GetPriceRange(int categoryId)
    {
        var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
        var minPrice = products.Min(p => p.Price);
        var maxPrice = products.Max(p => p.Price);

        var result = new
        {
            MinPrice = minPrice,
            MaxPrice = maxPrice
        };
        return Ok(result);
    }
}