using MediatR;
using MercerStore.Web.Application.Dtos.Review;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Reviews.Commands;

public record UpdateReviewCommand(CreateReviewDto CreateReviewDto) :
    LoggableRequest<Unit>("User update review", "review");

public class UpdateReviewHandler : IRequestHandler<UpdateReviewCommand, Unit>
{
    private readonly IReviewService _reviewService;
    private readonly IUserIdentifierService _userIdentifierService;

    public UpdateReviewHandler(IReviewService reviewService, IUserIdentifierService userIdentifierService)
    {
        _reviewService = reviewService;
        _userIdentifierService = userIdentifierService;
    }

    public async Task<Unit> Handle(UpdateReviewCommand request, CancellationToken ct)
    {
        var userId = _userIdentifierService.GetCurrentIdentifier();
        var review = await _reviewService.GetReview(request.CreateReviewDto.ProductId, userId, ct);

        review.Value = request.CreateReviewDto.Value;
        review.ReviewText = request.CreateReviewDto.ReviewText;
        review.EditDateTime = DateTime.UtcNow;
        review.Edited = true;

        await _reviewService.UpdateReview(review, ct);

        var logDetails = new
        {
            request.CreateReviewDto.ProductId,
            review.Value,
            review.ReviewText,
            review.Edited
        };

        request.EntityId = request.CreateReviewDto.ProductId;
        request.Details = logDetails;

        return Unit.Value;
    }
}