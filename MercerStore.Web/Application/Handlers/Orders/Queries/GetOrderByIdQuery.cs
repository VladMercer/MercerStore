using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Orders;

namespace MercerStore.Web.Application.Handlers.Orders.Queries
{
    public record GetOrderByIdQuery(int OrderId) : IRequest<Order>;
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order>
    {
        private readonly IOrderService _orderService;

        public GetOrderByIdHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetOrderById(request.OrderId);
        }
    }
}
