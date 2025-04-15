using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Areas.Admin.ViewModels.Orders;

namespace MercerStore.Web.Application.Handlers.Orders.Commands
{
    public record UpdateOderCommand(UpdateOrderViewModel UpdateOrderViewModel) :
        LoggableRequest<Unit>("Manager update order", "order");
    public class UpdateOrderHander : IRequestHandler<UpdateOderCommand, Unit>
    {
        private readonly IOrderService _orderService;

        public UpdateOrderHander(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<Unit> Handle(UpdateOderCommand request, CancellationToken cancellationToken)
        {
            await _orderService.UpdateOder(request.UpdateOrderViewModel);

            request.EntityId = request.UpdateOrderViewModel.Id;
            request.Details = new { request.UpdateOrderViewModel };

            return Unit.Value;
        }
    }
}
