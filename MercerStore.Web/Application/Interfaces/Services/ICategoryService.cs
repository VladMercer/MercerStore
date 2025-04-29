using MercerStore.Web.Application.Dtos.Product;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Categories;
using MercerStore.Web.Application.ViewModels.Categories;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<PaginatedResultDto<ProductDto>> GetFilteredProductsWithoutCache(CategoryFilterRequest request,
        CancellationToken ct);

    Task<PriceRangeDto> GetPriceRange(int categoryId, CancellationToken ct);
    Task<IEnumerable<Category>> GetAllCategories(CancellationToken ct);

    Task<Category> AddCategory(CreateCategoryViewModel createCategoryViewModel, string? categoryImgUrl,
        CancellationToken ct);

    Task<CategoryPageViewModel> GetCategoryPageViewModel(int categoryId, CancellationToken ct);
}
