using MediatR;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels;

namespace MercerStore.Web.Application.Handlers.Orders.Queries
{
    public record GetOrderViewModelQuery() : IRequest<OrderViewModel>;
    public class GetOrderViewModelHandler : IRequestHandler<GetOrderViewModelQuery, OrderViewModel>
    {
        private readonly IOrderService _orderService;
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly ICartService _cartService;

        public GetOrderViewModelHandler(IOrderService orderService, IUserIdentifierService userIdentifierService, ICartService cartService)
        {
            _orderService = orderService;
            _userIdentifierService = userIdentifierService;
            _cartService = cartService;
        }

        public async Task<OrderViewModel> Handle(GetOrderViewModelQuery request, CancellationToken cancellationToken)
        {
            var userId = _userIdentifierService.GetCurrentIdentifier();
            var cartViewModel = await _cartService.GetCartViewModel(userId);
            return await _orderService.GetOrderViewModel(userId, cartViewModel);
        }
    }
}
