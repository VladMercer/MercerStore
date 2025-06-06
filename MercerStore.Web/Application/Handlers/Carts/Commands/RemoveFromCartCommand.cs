﻿using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Carts.Commands;

public record RemoveFromCartCommand(int ProductId) : LoggableRequest<Unit>("User remove from cart", "Cart");

public class RemoveFromCartHandler : IRequestHandler<RemoveFromCartCommand, Unit>
{
    private readonly ICartService _cartService;
    private readonly IUserIdentifierService _userIdentifierService;

    public RemoveFromCartHandler(IUserIdentifierService userIdentifierService, ICartService cartService)
    {
        _userIdentifierService = userIdentifierService;
        _cartService = cartService;
    }

    public async Task<Unit> Handle(RemoveFromCartCommand request, CancellationToken ct)
    {
        var userId = _userIdentifierService.GetCurrentIdentifier();
        await _cartService.RemoveFromCart(request.ProductId, userId, ct);

        request.EntityId = request.ProductId;

        return Unit.Value;
    }
}
