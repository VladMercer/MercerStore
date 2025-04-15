using MediatR;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.UserDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Users;

namespace MercerStore.Web.Application.Handlers.Users.Queries
{
    public record GetFilteredUsersQuery(UserFilterRequest Request) : IRequest<PaginatedResultDto<UserDto>>;
    public class GetFilteredUsersHandler : IRequestHandler<GetFilteredUsersQuery, PaginatedResultDto<UserDto>>
    {
        private readonly IUserService _userService;
        private readonly IRedisCacheService _redisCacheService;

        public GetFilteredUsersHandler(IUserService userService, IRedisCacheService redisCacheService)
        {
            _userService = userService;
            _redisCacheService = redisCacheService;
        }

        public async Task<PaginatedResultDto<UserDto>> Handle(GetFilteredUsersQuery query, CancellationToken cancellationToken)
        {
            var request = query.Request;
            bool isDefaultQuery =
                request.PageNumber == 1 &&
                request.PageSize == 30 &&
                !request.SortOrder.HasValue &&
                !request.Period.HasValue &&
                !request.Filter.HasValue &&
                string.IsNullOrEmpty(request.Query);

            string cacheKey = $"users:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => _userService.GetFilteredSuppliersWithoutCache(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }
    }
}
