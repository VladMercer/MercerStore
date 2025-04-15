using MediatR;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Handlers.Reviews.Queries
{
    public record GetReviewQuery(int ProductId) : IRequest<Review>;
    public class GetReviewHandler : IRequestHandler<GetReviewQuery, Review>
    {
        private readonly IReviewService _reviewService;
        private readonly IUserIdentifierService _userIdentifierService;

        public GetReviewHandler(IReviewService reviewService, IUserIdentifierService userIdentifierService)
        {
            _reviewService = reviewService;
            _userIdentifierService = userIdentifierService;
        }

        public async Task<Review> Handle(GetReviewQuery request, CancellationToken cancellationToken)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            return await _reviewService.GetReview(request.ProductId, userId);
        }
    }
}
