namespace MercerStore.Web.Application.Dtos.Search;

public class SearchResultDto
{
    public IList<SearchProductDto>? Products { get; set; }
    public int? TotalItems { get; set; }
    public int? TotalPages { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}