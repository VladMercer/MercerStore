using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Orders.Commands;

public record RemoveOrderProductCommand(int OrderId, int ProductId) :
    LoggableRequest<Unit>("User removed product from order", "order");

public class RemoveOrderProductHandler : IRequestHandler<RemoveOrderProductCommand, Unit>
{
    private readonly IOrderService _orderService;

    public RemoveOrderProductHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<Unit> Handle(RemoveOrderProductCommand request, CancellationToken ct)
    {
        await _orderService.RemoveOrderProduct(request.OrderId, request.ProductId, ct);

        request.EntityId = request.OrderId;
        request.Details = new { request.ProductId };

        return Unit.Value;
    }
}
