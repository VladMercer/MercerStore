using MercerStore.Web.Application.Dtos.Product;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IElasticSearchService
{
    Task IndexProductAsync(Product product, CancellationToken ct);
    Task IndexProductsAsync(IEnumerable<Product> products, CancellationToken ct);
    Task<IEnumerable<ProductIndexDto>> SearchProductsAsync(string searchString, CancellationToken ct);
}
