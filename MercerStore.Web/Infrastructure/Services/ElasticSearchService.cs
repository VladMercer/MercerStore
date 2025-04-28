using System.Text;
using MercerStore.Web.Application.Dtos.Product;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using Nest;

namespace MercerStore.Web.Infrastructure.Services;

public class ElasticSearchService : IElasticSearchService
{
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<ElasticSearchService> _logger;

    public ElasticSearchService(IElasticClient elasticClient, ILogger<ElasticSearchService> logger)
    {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    public async Task IndexProductAsync(Product product, CancellationToken ct)
    {
        var indexProduct = new ProductIndexDto
        {
            Id = product.Id,
            Name = product.Name,
            SKU = product.SKU
        };

        var response = await _elasticClient.IndexDocumentAsync(indexProduct, ct);
        if (!response.IsValid) throw new Exception(response.OriginalException.Message);
    }

    public async Task IndexProductsAsync(IEnumerable<Product> products, CancellationToken ct)
    {
        var indexProducts = products.Select(x => new ProductIndexDto
        {
            Id = x.Id,
            Name = x.Name,
            SKU = x.SKU
        }).ToList();

        var bulkIndexResponse = await _elasticClient.BulkAsync(b => b
            .Index("product")
            .IndexMany(indexProducts), ct);

        if (!bulkIndexResponse.IsValid) throw new Exception(bulkIndexResponse.OriginalException.Message);
    }

    public async Task<IEnumerable<ProductIndexDto>> SearchProductsAsync(string searchString, CancellationToken ct)
    {
        var convertedQuery = ConvertLayout(searchString);

        var response = await _elasticClient.SearchAsync<ProductIndexDto>(s => s
                .Index("product")
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            sh => sh.Term(t => t.Field(f => f.Name).Value(searchString).Boost(5)),
                            sh => sh.Term(t => t.Field(f => f.Name).Value(convertedQuery).Boost(5)),
                            sh => sh.Match(m => m.Field(f => f.Name).Query(searchString)),
                            sh => sh.Match(m => m.Field(f => f.Name).Query(convertedQuery)),
                            sh => sh.Match(m => m.Field("name.ngram").Query(searchString).Fuzziness(Fuzziness.Auto)),
                            sh => sh.Match(m => m.Field("name.ngram").Query(convertedQuery).Fuzziness(Fuzziness.Auto))
                        )
                    )
                )
                .Highlight(h => h
                    .Fields(f => f
                        .Field(ff => ff.Name)
                        .Field("name.ngram")
                        .PreTags("<span class='highlighted'>")
                        .PostTags("</span>")
                    )
                )
                .Sort(s => s.Descending(SortSpecialField.Score))
                .Source(src => src
                    .Includes(i => i
                        .Fields(
                            f => f.Name,
                            f => f.Id,
                            f => f.SKU
                        )
                    )
                ), ct
        );

        _logger.LogInformation("Elastic response: {@Response}", response);

        return response.Hits.Select(hit => new ProductIndexDto
        {
            Id = hit.Source.Id,
            Name = hit.Highlight != null &&
                   (hit.Highlight.ContainsKey("name") || hit.Highlight.ContainsKey("name.ngram"))
                ? hit.Highlight.ContainsKey("name")
                    ? hit.Highlight["name"].FirstOrDefault()
                    : hit.Highlight["name.ngram"].FirstOrDefault()
                : hit.Source.Name,
            SKU = hit.Source.SKU
        });
    }

    private static string ConvertLayout(string input)
    {
        var ruToEn = new Dictionary<char, char>
        {
            { 'а', 'f' }, { 'б', ',' }, { 'в', 'd' }, { 'г', 'u' }, { 'д', 'l' }, { 'е', 't' }, { 'ё', '`' },
            { 'ж', ';' }, { 'з', 'p' }, { 'и', 'b' }, { 'й', 'q' }, { 'к', 'r' }, { 'л', 'k' }, { 'м', 'v' },
            { 'н', 'y' }, { 'о', 'j' }, { 'п', 'g' }, { 'р', 'h' }, { 'с', 'c' }, { 'т', 'n' }, { 'у', 'e' },
            { 'ф', 'a' }, { 'х', '[' }, { 'ц', 'w' }, { 'ч', 'x' }, { 'ш', 'i' }, { 'щ', 'o' }, { 'ъ', ']' },
            { 'ы', 's' }, { 'ь', 'm' }, { 'э', '\'' }, { 'ю', '.' }, { 'я', 'z' }
        };

        var enToRu = ruToEn.ToDictionary(kv => kv.Value, kv => kv.Key);

        var converted = new StringBuilder();
        foreach (var c in input)
            if (ruToEn.ContainsKey(c))
                converted.Append(ruToEn[c]);
            else if (enToRu.ContainsKey(c))
                converted.Append(enToRu[c]);
            else
                converted.Append(c);

        return converted.ToString();
    }
}