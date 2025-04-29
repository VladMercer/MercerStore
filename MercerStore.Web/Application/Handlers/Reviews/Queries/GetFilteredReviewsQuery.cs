using MediatR;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Dtos.Review;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Reviews;

namespace MercerStore.Web.Application.Handlers.Reviews.Queries;

public record GetFilteredReviewsQuery(ReviewFilterRequest Request) : IRequest<PaginatedResultDto<AdminReviewDto>>;

public class GetFilteredReviewsHandler : IRequestHandler<GetFilteredReviewsQuery, PaginatedResultDto<AdminReviewDto>>
{
    private readonly IRedisCacheService _redisCacheService;
    private readonly IReviewService _reviewService;

    public GetFilteredReviewsHandler(IReviewService reviewService, IRedisCacheService redisCacheService)
    {
        _reviewService = reviewService;
        _redisCacheService = redisCacheService;
    }

    public async Task<PaginatedResultDto<AdminReviewDto>> Handle(GetFilteredReviewsQuery query,
        CancellationToken ct)
    {
        var request = query.Request;
        var isDefaultQuery =
            request is { PageNumber: 1, PageSize: 30, SortOrder: null, Filter: null, Query: "" };

        const string cacheKey = "reviews:page1";

        return await _redisCacheService.TryGetOrSetCacheAsync(
            cacheKey,
            () => _reviewService.GetFilteredReviewsWithoutCache(request, ct),
            isDefaultQuery,
            TimeSpan.FromMinutes(10)
        );
    }
}
