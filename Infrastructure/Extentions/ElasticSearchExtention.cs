using MercerStore.Dtos.ProductDto;
using MercerStore.Models.Products;
using MercerStore.Models.Products;
using Nest;

namespace MercerStore.Infrastructure.Extentions
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
            settings.DefaultMappingFor<ProductIndexDto>(p =>
            p.PropertyName(p => p.Id, "id")
            .PropertyName(p => p.Name, "name")
            .PropertyName(p => p.SKU, "sku"));
        }
        private static void CreateIndex(IElasticClient client, string indexName)
        {
            client.Indices.Create(indexName, i => i.Map<ProductIndexDto>(x => x.AutoMap()));
        }
    }
}
