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
            request is { PageNumber: 1, PageSize: 30, SortOrder: null, Period: null, Filter: null } &&
            string.IsNullOrEmpty(request.Query);

        const string cacheKey = "users:page1";

        return await _redisCacheService.TryGetOrSetCacheAsync(
            cacheKey,
            () => _userService.GetFilteredSuppliersWithoutCache(request, ct),
            isDefaultQuery,
            TimeSpan.FromMinutes(10)
        );
    }
}