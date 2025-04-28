using MercerStore.Web.Application.Dtos.Product;
using MercerStore.Web.Infrastructure.Helpers;
using Nest;

namespace MercerStore.Web.Infrastructure.Extensions;

public static class ElasticSearchExtension
{
    public static void AddElasticSearch(
        this IServiceCollection services, ElasticConfiguration elasticConfiguration)
    {
        var settings = new ConnectionSettings(new Uri(elasticConfiguration.Uri))
            .PrettyJson()
            .DefaultIndex(elasticConfiguration.Index);

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
