using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Areas.Admin.ViewModels.Orders;
using MercerStore.Web.Infrastructure.Data.Enum.Order;

namespace MercerStore.Web.Application.Handlers.Orders.Commands;

public record UpdateOderCommand(UpdateOrderViewModel UpdateOrderViewModel) :
    LoggableRequest<Unit>("Manager update order", "order");

public class UpdateOrderHandler : IRequestHandler<UpdateOderCommand, Unit>
{
    private readonly IOrderService _orderService;
    private readonly IDateTimeConverter _dateTimeConverter;

    public UpdateOrderHandler(IOrderService orderService, IDateTimeConverter dateTimeConverter)
    {
        _orderService = orderService;
        _dateTimeConverter = dateTimeConverter;
    }

    public async Task<Unit> Handle(UpdateOderCommand request, CancellationToken ct)
    {
        request.UpdateOrderViewModel.CreateDate =
            _dateTimeConverter.ConvertUserTimeToUtc(request.UpdateOrderViewModel.CreateDate);

        if (request.UpdateOrderViewModel.CompletedDate.HasValue)
        {
            request.UpdateOrderViewModel.CompletedDate =
                _dateTimeConverter.ConvertUserTimeToUtc(request.UpdateOrderViewModel.CompletedDate.Value);
        }

        await _orderService.UpdateOder(request.UpdateOrderViewModel, ct);

        request.EntityId = request.UpdateOrderViewModel.Id;
        request.Details = new { request.UpdateOrderViewModel };

        return Unit.Value;
    }
}