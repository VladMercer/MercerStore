using MercerStore.Models;

namespace MercerStore.Interfaces
{
	public interface IElasticSearchService
	{
		Task IndexProductAsync(Product product);
		Task IndexProductsAsync(IEnumerable<Product> products);
		Task<IEnumerable<Product>> SearchProductsAsync(string searchString);
	}
}
