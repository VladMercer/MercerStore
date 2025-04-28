using MercerStore.Web.Application.Dtos.Product;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Products;
using MercerStore.Web.Application.ViewModels.Products;
using MercerStore.Web.Areas.Admin.ViewModels.Products;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IProductService
{
    Task<PaginatedResultDto<AdminProductDto>> GetFilteredProductsWithoutCache(ProductFilterRequest request,
        CancellationToken ct);

    Task<Product> GetProductById(int productId, CancellationToken ct);
    Task<ProductViewModel> GetProductDetails(int productId, CancellationToken ct);
    Task<IEnumerable<Category>> GetAllCategories(CancellationToken ct);
    Task AddProduct(Product product, CancellationToken ct);
    Task<UpdateProductViewModel> GetUpdateProductViewModel(int productId, CancellationToken ct);
    Task UpdateProduct(Product product, CancellationToken ct);
    Task<IEnumerable<Product>> GetAllProducts(CancellationToken ct);
}