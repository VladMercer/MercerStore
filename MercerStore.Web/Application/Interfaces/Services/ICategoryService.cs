using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Categories;
using MercerStore.Web.Application.ViewModels.Categories;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<PaginatedResultDto<ProductDto>> GetFilteredProducts(CateroryFilterRequest request);
        Task<PriceRangeDto> GetPriceRange(int categoryId);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<int> AddCategory(CreateCategoryViewModel createCategoryViewModel);
        Task<CategoryPageViewModel> GetCategoryPageViewModel(int categoryId);
    }
}
