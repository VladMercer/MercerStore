using MercerStore.Interfaces;
using MercerStore.Models;
using Nest;

namespace MercerStore.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IElasticClient _elasticClient;
        public ElasticSearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task IndexProductAsync(Product product)
        {
            var response = await _elasticClient.IndexDocumentAsync(product);
            if (!response.IsValid)
            {
                throw new Exception(response.OriginalException.Message);
            }
        }

        public async Task IndexProductsAsync(IEnumerable<Product> products)
        {
            var bulkIndexResponse = await _elasticClient.BulkAsync(b => b
            .Index("product")
            .IndexMany(products));

            if (!bulkIndexResponse.IsValid)
            {
                throw new Exception(bulkIndexResponse.OriginalException.Message);
            }
        }
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchString)
        {
            var response = await _elasticClient.SearchAsync<Product>(s => s
                .Index("product_v3")
                .Query(q => q
                    .MultiMatch(m => m
                        .Fields(f => f
                            .Field(ff => ff.Name)
                            .Field(ff => ff.Description)
                            .Field("name.ngram")       
                            .Field("description.ngram") 
                        )
                        .Query(searchString)
                        .Fuzziness(Fuzziness.Auto)  
                    )
                )
            );

            return response.Documents;
        }
    }
}