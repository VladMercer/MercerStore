using MediatR;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Carts;

namespace MercerStore.Web.Application.Handlers.Carts.Queries
{
    public record GetCartViewModelQuery() : IRequest<CartViewModel>;
    public class GetCartViewModelHandler : IRequestHandler<GetCartViewModelQuery, CartViewModel>
    {
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly ICartService _cartService;

        public GetCartViewModelHandler(IUserIdentifierService userIdentifierService, ICartService cartService)
        {
            _userIdentifierService = userIdentifierService;
            _cartService = cartService;
        }

        public async Task<CartViewModel> Handle(GetCartViewModelQuery request, CancellationToken cancellationToken)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            return await _cartService.GetCartViewModel(userId);
        }
    }
}
