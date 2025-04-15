using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Reviews.Commands
{
    public record RemoveReviewByIdCommand(int ReviewId) :
        LoggableRequest<Unit>("User remove review", "review");
    public class RemoveReviewByIdHandler : IRequestHandler<RemoveReviewByIdCommand, Unit>
    {
        private readonly IReviewService _reviewService;

        public RemoveReviewByIdHandler(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<Unit> Handle(RemoveReviewByIdCommand request, CancellationToken cancellationToken)
        {
            await _reviewService.RemoveReviewById(request.ReviewId);
            return Unit.Value;
        }
    }
}
