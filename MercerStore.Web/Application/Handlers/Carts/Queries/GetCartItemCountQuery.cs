using MediatR;
using MercerStore.Web.Application.Handlers.Carts.Queries;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.ViewModels.Carts;
namespace MercerStore.Web.Application.Handlers.Cart.Queries
{
    public record GetCartItemCountQuery() : IRequest<int?>;
    public class GetCartItemCountHandler : IRequestHandler<GetCartItemCountQuery, int?>
    {
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly ICartService _cartService;

        public GetCartItemCountHandler(IUserIdentifierService userIdentifierService, ICartService cartService)
        {
            _userIdentifierService = userIdentifierService;
            _cartService = cartService;
        }

        public async Task<int?> Handle(GetCartItemCountQuery request, CancellationToken cancellationToken)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            return await _cartService.GetCartItemCount(userId);
        }
    }
}
