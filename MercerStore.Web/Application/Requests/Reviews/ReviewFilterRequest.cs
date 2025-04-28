using MercerStore.Web.Infrastructure.Data.Enum;
using MercerStore.Web.Infrastructure.Data.Enum.Review;

namespace MercerStore.Web.Application.Requests.Reviews;

public record ReviewFilterRequest(
    int PageNumber = 1,
    int PageSize = 9,
    ReviewSortOrder? SortOrder = null,
    TimePeriod? Period = null,
    ReviewFilter? Filter = null,
    string? Query = null);