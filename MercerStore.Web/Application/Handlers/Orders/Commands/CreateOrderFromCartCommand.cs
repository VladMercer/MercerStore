using MediatR;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Application.ViewModels;

namespace MercerStore.Web.Application.Handlers.Orders.Commands
{
    public record CreateOrderFromCartCommand(OrderViewModel OrderViewModel) :
        LoggableRequest<Unit>("User created an order", "order");
    public class CreateOrderFromCartHandler : IRequestHandler<CreateOrderFromCartCommand, Unit>
    {
        private readonly IOrderService _orderService;
        private readonly IUserIdentifierService _userIdentifierService;

        public CreateOrderFromCartHandler(IOrderService orderService, IUserIdentifierService userIdentifierService)
        {
            _orderService = orderService;
            _userIdentifierService = userIdentifierService;
        }

        public async Task<Unit> Handle(CreateOrderFromCartCommand request, CancellationToken cancellationToken)
        {
            var roles = _userIdentifierService.GetCurrentUserRoles();
            var userId = _userIdentifierService.GetCurrentIdentifier();

            var result = await _orderService.CreateOrderFromCart(request.OrderViewModel, userId, roles);

            request.EntityId = result.Id;
            request.Details = new { result };

            return Unit.Value;
        }
    }
}
