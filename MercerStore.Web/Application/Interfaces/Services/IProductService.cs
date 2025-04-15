using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Products;
using MercerStore.Web.Application.ViewModels.Products;
using MercerStore.Web.Areas.Admin.ViewModels.Products;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<PaginatedResultDto<AdminProductDto>> GetFilteredProductsWithoutCache(ProductFilterRequest request);
        Task<Product> GetProductById(int productId);
        Task<ProductViewModel> GetProductDetails(int productId);
        Task<IEnumerable<Category>> GetAllCategories();
        Task AddProduct(Product product);
        Task<UpdateProductViewModel> GetUpdateProductViewModel(int productId);
        Task UpdateProduct(Product product);
        Task<IEnumerable<Product>> GetAllProducts();
    }
}
