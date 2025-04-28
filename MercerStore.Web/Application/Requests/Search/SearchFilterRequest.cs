namespace MercerStore.Web.Application.Requests.Search;

public record SearchFilterRequest(string Query, string? SortOrder, int? PageNumber, int? PageSize);