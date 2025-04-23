using MediatR;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Cart.Commands;

public record AddToCartCommand(int ProductId) :
    LoggableRequest<Unit>("User add to cart", "Cart");

public class AddToCartHandler : IRequestHandler<AddToCartCommand, Unit>
{
    private readonly ICartService _cartService;
    private readonly IUserIdentifierService _userIdentifierService;

    public AddToCartHandler(IUserIdentifierService userIdentifierService, ICartService cartService)
    {
        _userIdentifierService = userIdentifierService;
        _cartService = cartService;
    }

    public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var userId = _userIdentifierService.GetCurrentIdentifier();
        await _cartService.AddToCart(request.ProductId, userId);
        request.EntityId = request.ProductId;
        return Unit.Value;
    }
}