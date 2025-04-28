using MercerStore.Web.Infrastructure.Data.Enum;
using MercerStore.Web.Infrastructure.Data.Enum.Order;

namespace MercerStore.Web.Application.Requests.Orders;

public record OrderFilterRequest(
    int PageNumber = 1,
    int PageSize = 9,
    OrdersSortOrder? SortOrder = null,
    TimePeriod? Period = null,
    OrderStatuses? Status = null,
    string? Query = null);