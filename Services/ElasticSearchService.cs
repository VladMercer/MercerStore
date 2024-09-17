using MercerStore.Interfaces;
using MercerStore.Models;
using Nest;
using System.Text;

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
			string convertedQuery = ConvertLayout(searchString); 

			var response = await _elasticClient.SearchAsync<Product>(s => s
				.Index("product")
				.Query(q => q
					.MultiMatch(m => m
						.Fields(f => f
							.Field(ff => ff.Name, boost: 3)  
							.Field("name.ngram", boost: 1)   
						)
						.Query($"{searchString} {convertedQuery}")  
						.Fuzziness(Fuzziness.Auto)
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
				.Source(src => src
					.Includes(i => i
						.Fields(
							f => f.Name,
							f => f.Price,
							f => f.Description,
							f => f.MainImageUrl,
							f => f.Id,
							f => f.CategoryId,
							f => f.Category
						)
					)
				)
			);

			return response.Hits.Select(hit => new Product
			{
				Id = hit.Source.Id,
				Name = hit.Highlight != null && (hit.Highlight.ContainsKey("name") || hit.Highlight.ContainsKey("name.ngram"))
			   ? hit.Highlight.ContainsKey("name")
				   ? hit.Highlight["name"].FirstOrDefault()
				   : hit.Highlight["name.ngram"].FirstOrDefault()
			   : hit.Source.Name,
				Description = hit.Source.Description,
				Price = hit.Source.Price,
				MainImageUrl = hit.Source.MainImageUrl,
				CategoryId = hit.Source.CategoryId,
				Category = hit.Source.Category
			});
		}
		private string ConvertLayout(string input)
		{
			var ruToEn = new Dictionary<char, char>
	{
		{'а', 'f'}, {'б', ','}, {'в', 'd'}, {'г', 'u'}, {'д', 'l'}, {'е', 't'}, {'ё', '`'},
		{'ж', ';'}, {'з', 'p'}, {'и', 'b'}, {'й', 'q'}, {'к', 'r'}, {'л', 'k'}, {'м', 'v'},
		{'н', 'y'}, {'о', 'j'}, {'п', 'g'}, {'р', 'h'}, {'с', 'c'}, {'т', 'n'}, {'у', 'e'},
		{'ф', 'a'}, {'х', '['}, {'ц', 'w'}, {'ч', 'x'}, {'ш', 'i'}, {'щ', 'o'}, {'ъ', ']'},
		{'ы', 's'}, {'ь', 'm'}, {'э', '\''}, {'ю', '.'}, {'я', 'z'}
	};

			var enToRu = ruToEn.ToDictionary(kv => kv.Value, kv => kv.Key);

			StringBuilder converted = new StringBuilder();
			foreach (char c in input)
			{
				if (ruToEn.ContainsKey(c))
				{
					converted.Append(ruToEn[c]);
				}
				else if (enToRu.ContainsKey(c))
				{
					converted.Append(enToRu[c]);
				}
				else
				{
					converted.Append(c);
				}
			}

			return converted.ToString();
		}
	}
}


