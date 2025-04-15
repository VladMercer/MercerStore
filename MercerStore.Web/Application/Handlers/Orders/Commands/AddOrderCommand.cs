using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Orders;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Orders.Commands
{
    public record AddOrderCommand(Order Order) : LoggableRequest<Unit>("User created an order", "order");
    public class AddOrderHandler : IRequestHandler<AddOrderCommand, Unit>
    {
        private readonly IOrderService _orderService;

        public AddOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Unit> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var result = await _orderService.AddOrder(request.Order);

            request.EntityId = result.Id;
            request.Details = new { result };

            return Unit.Value;
        }
    }
}
