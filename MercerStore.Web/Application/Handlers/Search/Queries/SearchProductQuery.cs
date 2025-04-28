using MediatR;
using MercerStore.Web.Application.Dtos.Search;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Search;

namespace MercerStore.Web.Application.Handlers.Search.Queries;

public record SearchProductQuery(SearchFilterRequest Request) : IRequest<SearchResultDto>;

public class SearchProductHandler : IRequestHandler<SearchProductQuery, SearchResultDto>
{
    private readonly IElasticSearchService _elasticSearchService;
    private readonly ISearchService _searchService;

    public SearchProductHandler(ISearchService searchService, IElasticSearchService elasticSearchService)
    {
        _searchService = searchService;
        _elasticSearchService = elasticSearchService;
    }

    public async Task<SearchResultDto> Handle(SearchProductQuery request, CancellationToken ct)
    {
        var productIndexDto = await _elasticSearchService.SearchProductsAsync(request.Request.Query, ct);
        return await _searchService.SearchProduct(productIndexDto, request.Request, ct);
    }
}