using MediatR;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Reviews.Queries;

public record GetAvgRateProductQuery(int ProductId) : IRequest<double>;

public class GetAvgRateProductHandler : IRequestHandler<GetAvgRateProductQuery, double>
{
    private readonly IReviewService _reviewService;

    public GetAvgRateProductHandler(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    public async Task<double> Handle(GetAvgRateProductQuery request, CancellationToken ct)
    {
        return await _reviewService.GetAvgRateProduct(request.ProductId, ct);
    }
}