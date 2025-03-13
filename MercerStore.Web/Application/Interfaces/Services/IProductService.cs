using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Products;
using MercerStore.Web.Application.ViewModels.Products;
using MercerStore.Web.Areas.Admin.ViewModels.Products;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<PaginatedResultDto<AdminProductDto>> GetAdminFilteredProducts(ProductFilterRequest request);
        Task<Product> GetProduct(int productId);
        Task<string> GetProductSku(int productId);
        Task<ProductViewModel> GetProductDetails(int productId);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<int> CreateProduct(CreateProductViewModel createViewModel, int categoryId);
        void UpdateSkus();
        Task<UpdateProductViewModel> GetUpdateProductViewModel(int productId);
        Task<Result<int>> UpdateProduct(UpdateProductViewModel updateProductViewModel);
        Task IndexAllProducts();
    }
}
