using MediatR;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Reviews.Commands
{
    public record RemoveReviewCommand(int ProductId) : LoggableRequest<Unit>("User remove review", "review");
    public class RemoveReviewHandler : IRequestHandler<RemoveReviewCommand, Unit>
    {
        private readonly IReviewService _reviewService;
        private readonly IUserIdentifierService _userIdentifierService;

        public RemoveReviewHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<Unit> Handle(RemoveReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();

            var review = await _reviewService.GetReview(request.ProductId, userId);

            await _reviewService.RemoveReviewById(review.Id);

            request.EntityId = review.Id;
            request.Details = new
            {
                review.Value,
                review.ProductId,
                review.ReviewText
            };

            return Unit.Value;
        }
    }
}
