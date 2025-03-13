namespace MercerStore.Web.Application.Dtos.SearchDto
{
    public class SearchResultDto
    {
        public List<SearchProductDto> Products { get; set; }
        public int? TotalItems { get; set; }
        public int? TotalPages { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
