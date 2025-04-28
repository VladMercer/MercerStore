using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Carts;

namespace MercerStore.Web.Application.Handlers.Carts.Queries;

public record GetCartViewModelQuery : IRequest<CartViewModel>;

public class GetCartViewModelHandler : IRequestHandler<GetCartViewModelQuery, CartViewModel>
{
    private readonly ICartService _cartService;
    private readonly IUserIdentifierService _userIdentifierService;

    public GetCartViewModelHandler(IUserIdentifierService userIdentifierService, ICartService cartService)
    {
        _userIdentifierService = userIdentifierService;
        _cartService = cartService;
    }

    public async Task<CartViewModel> Handle(GetCartViewModelQuery request, CancellationToken ct)
    {
        var userId = _userIdentifierService.GetCurrentIdentifier();
        return await _cartService.GetCartViewModel(userId, ct);
    }
}