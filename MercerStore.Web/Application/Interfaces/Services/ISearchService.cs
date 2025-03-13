using MercerStore.Web.Application.Dtos.SearchDto;
using MercerStore.Web.Application.Requests.Search;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface ISearchService
    {
        Task<SearchResultDto> SearchProduct(SearchFilterRequest request);
    }
}
