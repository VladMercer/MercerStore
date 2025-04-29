using MercerStore.Web.Infrastructure.Data.Enum;
using MercerStore.Web.Infrastructure.Data.Enum.User;

namespace MercerStore.Web.Application.Requests.Users;

public record UserFilterRequest(
    int PageNumber,
    int PageSize,
    UserSortOrder? SortOrder,
    TimePeriod? Period,
    UserFilter? Filter,
    string? Query);
