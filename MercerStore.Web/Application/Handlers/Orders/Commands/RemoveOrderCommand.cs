using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Orders.Commands;

public record RemoveOrderCommand(int OrderId) : LoggableRequest<Unit>("User remove order", "order");

public class RemoveOrderHandler : IRequestHandler<RemoveOrderCommand, Unit>
{
    private readonly IOrderService _orderService;

    public RemoveOrderHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<Unit> Handle(RemoveOrderCommand request, CancellationToken ct)
    {
        await _orderService.RemoveOrder(request.OrderId, ct);
        request.EntityId = request.OrderId;

        return Unit.Value;
    }
}
