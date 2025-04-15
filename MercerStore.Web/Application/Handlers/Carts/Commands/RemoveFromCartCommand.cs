using MediatR;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Cart.Commands
{
    public record RemoveFromCartCommand(int ProductId) : LoggableRequest<Unit>("User remove from cart", "Cart");
    public class RemoveFromCartHandler : IRequestHandler<RemoveFromCartCommand, Unit>
    {
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly ICartService _cartService;
        public RemoveFromCartHandler(IUserIdentifierService userIdentifierService, ICartService cartService)
        {
            _userIdentifierService = userIdentifierService;
            _cartService = cartService;
        }

        public async Task<Unit> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            await _cartService.RemoveFromCart(request.ProductId, userId);

            request.EntityId = request.ProductId;

            return Unit.Value;
        }
    }
}
