using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Handlers.Reviews.Queries;

public record GetReviewQuery(int ProductId) : IRequest<Review>;

public class GetReviewHandler : IRequestHandler<GetReviewQuery, Review>
{
    private readonly IDateTimeConverter _dateTimeConverter;
    private readonly IReviewService _reviewService;
    private readonly IUserIdentifierService _userIdentifierService;

    public GetReviewHandler(IReviewService reviewService, IUserIdentifierService userIdentifierService,
        IDateTimeConverter dateTimeConverter)
    {
        _reviewService = reviewService;
        _userIdentifierService = userIdentifierService;
        _dateTimeConverter = dateTimeConverter;
    }

    public async Task<Review> Handle(GetReviewQuery request, CancellationToken ct)
    {
        var userId = _userIdentifierService.GetCurrentIdentifier();
        var review = await _reviewService.GetReview(request.ProductId, userId, ct);

        review.Date = _dateTimeConverter.ConvertUtcToUserTime(review.Date);
        review.EditDateTime = _dateTimeConverter.ConvertUtcToUserTime(review.EditDateTime);

        return review;
    }
}
