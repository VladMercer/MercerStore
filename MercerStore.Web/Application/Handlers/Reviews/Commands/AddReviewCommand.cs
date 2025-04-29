using MediatR;
using MercerStore.Web.Application.Dtos.Review;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Reviews.Commands;

public record AddReviewCommand(CreateReviewDto CreateReviewDto) :
    LoggableRequest<Result<Unit>>("User left a review", "review");

public class AddReviewHandler : IRequestHandler<AddReviewCommand, Result<Unit>>
{
    private readonly IReviewService _reviewService;
    private readonly IUserIdentifierService _userIdentifierService;

    public AddReviewHandler(IReviewService reviewService, IUserIdentifierService userIdentifierService)
    {
        _reviewService = reviewService;
        _userIdentifierService = userIdentifierService;
    }

    public async Task<Result<Unit>> Handle(AddReviewCommand request, CancellationToken ct)
    {
        var userId = _userIdentifierService.GetCurrentIdentifier();
        var existingReview = await _reviewService.GetReview(request.CreateReviewDto.ProductId, userId, ct);

        if (existingReview != null) return Result<Unit>.Failure("Отзыв уже существует");

        var review = new Review
        {
            UserId = userId,
            ReviewText = request.CreateReviewDto.ReviewText,
            ProductId = request.CreateReviewDto.ProductId,
            Value = request.CreateReviewDto.Value,
            Date = DateTime.UtcNow,
            EditDateTime = DateTime.UtcNow
        };

        await _reviewService.AddReview(review, ct);

        var logDetails = new
        {
            review.ProductId,
            review.Value,
            review.ReviewText
        };

        request.EntityId = review.Id;
        request.Details = logDetails;

        return Result<Unit>.Success(Unit.Value);
    }
}
