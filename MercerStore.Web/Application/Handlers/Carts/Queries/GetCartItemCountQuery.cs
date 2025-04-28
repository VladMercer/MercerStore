using MediatR;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Carts.Queries;

public record GetCartItemCountQuery : IRequest<int?>;

public class GetCartItemCountHandler : IRequestHandler<GetCartItemCountQuery, int?>
{
    private readonly ICartService _cartService;
    private readonly IUserIdentifierService _userIdentifierService;

    public GetCartItemCountHandler(IUserIdentifierService userIdentifierService, ICartService cartService)
    {
        _userIdentifierService = userIdentifierService;
        _cartService = cartService;
    }

    public async Task<int?> Handle(GetCartItemCountQuery request, CancellationToken ct)
    {
        var userId = _userIdentifierService.GetCurrentIdentifier();
        return await _cartService.GetCartItemCount(userId, ct);
    }
}