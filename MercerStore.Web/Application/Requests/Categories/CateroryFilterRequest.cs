namespace MercerStore.Web.Application.Requests.Categories;

public record CategoryFilterRequest(
    int CategoryId,
    int PageNumber = 1,
    int PageSize = 9,
    string? SortOrder = null,
    int? MaxPrice = null,
    int? MinPrice = null);