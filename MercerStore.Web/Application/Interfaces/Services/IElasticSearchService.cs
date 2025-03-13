using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Interfaces
{
    public interface IElasticSearchService
    {
        Task IndexProductAsync(Product product);
        Task IndexProductsAsync(IEnumerable<Product> products);
        Task<IEnumerable<ProductIndexDto>> SearchProductsAsync(string searchString);
    }
}
