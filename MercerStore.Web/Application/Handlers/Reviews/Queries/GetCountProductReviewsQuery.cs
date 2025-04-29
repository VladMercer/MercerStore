using MediatR;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Reviews.Queries;

public record GetCountProductReviewsQuery(int ProductId) : IRequest<int>;

public class GetCountProductReviewsHandler : IRequestHandler<GetCountProductReviewsQuery, int>
{
    private readonly IReviewService _reviewService;

    public GetCountProductReviewsHandler(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    public async Task<int> Handle(GetCountProductReviewsQuery request, CancellationToken ct)
    {
        return await _reviewService.GetCountProductReviews(request.ProductId, ct);
    }
}
