using MercerStore.Models;
using Nest;

namespace MercerStore.Extentions
{
	public static class ElasticSearchExtention
	{
		public static void AddElasticSearch(
			this IServiceCollection services, IConfiguration configuration)
		{
			var url = configuration["ElasticConfiguration:Uri"];
			var defaultIndex = configuration["ElasticConfiguration:Index"];

			var settings = new ConnectionSettings(new Uri(url))
				.PrettyJson()
				.DefaultIndex(defaultIndex);

			AddDefaultMappings(settings);

			var client = new ElasticClient(settings);
			services.AddSingleton<IElasticClient>(client);
		}
		public static void AddDefaultMappings(ConnectionSettings settings)
		{
			settings.DefaultMappingFor<Product>(p =>
			p.PropertyName(p => p.Id, "id")
			.PropertyName(p => p.Name, "name")
			.PropertyName(p => p.Price, "price")
			.PropertyName(p => p.Description, "description")
			.PropertyName(p => p.MainImageUrl, "mainImageUrl")
			.PropertyName(p => p.CategoryId, "categoryId")
			.PropertyName(p => p.Category, "category"));
		}
		private static void CreateIndex(IElasticClient client, string indexName)
		{
			client.Indices.Create(indexName, i => i.Map<Product>(x => x.AutoMap()));
		}
	}
}
