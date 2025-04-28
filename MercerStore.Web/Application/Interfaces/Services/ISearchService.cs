using MercerStore.Web.Application.Dtos.Product;
using MercerStore.Web.Application.Dtos.Search;
using MercerStore.Web.Application.Requests.Search;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface ISearchService
{
    Task<SearchResultDto> SearchProduct(IEnumerable<ProductIndexDto> productIndexDto, SearchFilterRequest request,
        CancellationToken ct);
}