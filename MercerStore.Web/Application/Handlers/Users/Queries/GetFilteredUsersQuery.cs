using MediatR;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Dtos.User;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Users;

namespace MercerStore.Web.Application.Handlers.Users.Queries;

public record GetFilteredUsersQuery(UserFilterRequest Request) : IRequest<PaginatedResultDto<UserDto>>;

public class GetFilteredUsersHandler : IRequestHandler<GetFilteredUsersQuery, PaginatedResultDto<UserDto>>
{
    private readonly IRedisCacheService _redisCacheService;
    private readonly IUserService _userService;

    public GetFilteredUsersHandler(IUserService userService, IRedisCacheService redisCacheService)
    {
        _userService = userService;
        _redisCacheService = redisCacheService;
    }

    public async Task<PaginatedResultDto<UserDto>> Handle(GetFilteredUsersQuery query,
        CancellationToken ct)
    {
        var request = query.Request;
        var isDefaultQuery =
            request.PageNumber == 1 &&
            request.PageSize == 30 &&
            !request.SortOrder.HasValue &&
            !request.Period.HasValue &&
            !request.Filter.HasValue &&
            string.IsNullOrEmpty(request.Query);

        var cacheKey = "users:page1";

        return await _redisCacheService.TryGetOrSetCacheAsync(
            cacheKey,
            () => _userService.GetFilteredSuppliersWithoutCache(request, ct),
            isDefaultQuery,
            TimeSpan.FromMinutes(10)
        );
    }
}