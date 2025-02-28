using MercerStore.Dtos.ProductDto;
using MercerStore.Models.Products;
using MercerStore.Models.Products;

namespace MercerStore.Interfaces
{
    public interface IElasticSearchService
	{
		Task IndexProductAsync(Product product);
		Task IndexProductsAsync(IEnumerable<Product> products);
		Task<IEnumerable<ProductIndexDto>> SearchProductsAsync(string searchString);
	}
}
