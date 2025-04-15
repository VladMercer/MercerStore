using MediatR;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.ReviewDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Reviews;

namespace MercerStore.Web.Application.Handlers.Reviews.Queries
{
    public record GetFilteredReviewsQuery(ReviewFilterRequest Request) : IRequest<PaginatedResultDto<AdminReviewDto>>;
    public class GetFilteredReviewsHandler : IRequestHandler<GetFilteredReviewsQuery, PaginatedResultDto<AdminReviewDto>>
    {
        private readonly IReviewService _reviewService;
        private readonly IRedisCacheService _redisCacheService;

        public GetFilteredReviewsHandler(IReviewService reviewService, IRedisCacheService redisCacheService)
        {
            _reviewService = reviewService;
            _redisCacheService = redisCacheService;
        }

        public async Task<PaginatedResultDto<AdminReviewDto>> Handle(GetFilteredReviewsQuery query, CancellationToken cancellationToken)
        {
            var request = query.Request;
            bool isDefaultQuery =
                request.PageNumber == 1 &&
                request.PageSize == 30 &&
                !request.SortOrder.HasValue &&
                !request.Filter.HasValue &&
                request.Query == "";

            string cacheKey = $"reviews:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => _reviewService.GetFilteredReviewsWithoutCache(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }
    }
}
