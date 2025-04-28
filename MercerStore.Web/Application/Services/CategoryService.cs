using MercerStore.Web.Application.Dtos.Product;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Categories;
using MercerStore.Web.Application.ViewModels.Categories;

namespace MercerStore.Web.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public CategoryService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<PaginatedResultDto<ProductDto>> GetFilteredProductsWithoutCache(CategoryFilterRequest request,
        CancellationToken ct)
    {
        var products = await _productRepository.GetProductsByCategoryAsync(request.CategoryId, ct);

        if (request.MinPrice.HasValue)
            products = products.Where(p => p.ProductPricing.OriginalPrice >= request.MinPrice.Value);
        if (request.MaxPrice.HasValue)
            products = products.Where(p => p.ProductPricing.OriginalPrice <= request.MaxPrice.Value);

        products = request.SortOrder switch
        {
            "name_desc" => products.OrderByDescending(p => p.Name, StringComparer.Ordinal),
            "price_asc" => products.OrderBy(p => p.ProductPricing.OriginalPrice),
            "price_desc" => products.OrderByDescending(p => p.ProductPricing.OriginalPrice),
            _ => products.OrderBy(p => p.Name, StringComparer.Ordinal)
        };

        var totalItems = products.Count();

        var pageProducts = products
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.ProductPricing.OriginalPrice,
                MainImageUrl = p.MainImageUrl,
                CategoryId = p.CategoryId,
                Description = p.ProductDescription.DescriptionText,
                DiscountedPrice = p.ProductPricing.DiscountedPrice
            });

        return new PaginatedResultDto<ProductDto>(pageProducts, totalItems, request.PageSize);
    }

    public async Task<PriceRangeDto> GetPriceRange(int categoryId, CancellationToken ct)
    {
        var products = await _categoryRepository.GetProductsByCategoryId(categoryId, ct);
        return new PriceRangeDto
        {
            MaxPrice = products.Max(p => p.ProductPricing.DiscountedPrice ?? p.ProductPricing.OriginalPrice),
            MinPrice = products.Min(p => p.ProductPricing.DiscountedPrice ?? p.ProductPricing.OriginalPrice)
        };
    }

    public async Task<IEnumerable<Category>> GetAllCategories(CancellationToken ct)
    {
        return await _categoryRepository.GetAllCategoriesAsync(ct);
    }

    public async Task<Category> AddCategory(CreateCategoryViewModel createCategoryViewModel, string? categoryImgUrl,
        CancellationToken ct)
    {
        var category = new Category
        {
            Name = createCategoryViewModel.Name,
            Description = createCategoryViewModel.Description,
            CategoryImgUrl = categoryImgUrl
        };

        return await _categoryRepository.AddCategory(category, ct);
    }

    public async Task<CategoryPageViewModel> GetCategoryPageViewModel(int categoryId, CancellationToken ct)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(categoryId, ct);
        return new CategoryPageViewModel
        {
            Category = category,
            SelectedCategoryId = categoryId
        };
    }
}
