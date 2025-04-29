using MediatR;
using MercerStore.Web.Application.Dtos.Order;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Orders.Commands;

public record UpdateOrderStatusCommand(UpdateOrderStatusDto UpdateOrderStatusDto) :
    LoggableRequest<Unit>("User update order status", "order");

public class UpdateOderStatusHanlder : IRequestHandler<UpdateOrderStatusCommand, Unit>
{
    private readonly IOrderService _orderService;

    public UpdateOderStatusHanlder(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken ct)
    {
        await _orderService.UpdateOrderStatus(request.UpdateOrderStatusDto, ct);

        request.EntityId = request.UpdateOrderStatusDto.OrderId;
        request.Details = new { request.UpdateOrderStatusDto };

        return Unit.Value;
    }
}
