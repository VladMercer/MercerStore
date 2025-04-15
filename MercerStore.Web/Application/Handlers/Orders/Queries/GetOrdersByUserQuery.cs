using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Orders;

namespace MercerStore.Web.Application.Handlers.Orders.Queries
{
    public record GetOrdersByUserQuery(string UserId) : IRequest<IEnumerable<Order>>;
    public class GetOrdersByUserHandler : IRequestHandler<GetOrdersByUserQuery, IEnumerable<Order>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersByUserHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IEnumerable<Order>> Handle(GetOrdersByUserQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetOrdersByUserId(request.UserId);
        }
    }
}
