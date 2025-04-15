using MediatR;
using MercerStore.Web.Application.Dtos.ReviewDto;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Reviews.Queries
{
    public record GetProductReviewsQuery(int ProductId) : IRequest<IEnumerable<ReviewDto>>;
    public class GetProductReviewsHandler : IRequestHandler<GetProductReviewsQuery, IEnumerable<ReviewDto>>
    {
        private readonly IReviewService _reviewService;

        public GetProductReviewsHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<IEnumerable<ReviewDto>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
        {
            return await _reviewService.GetProductReviews(request.ProductId);
        }
    }
}
