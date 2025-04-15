using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Orders;

namespace MercerStore.Web.Application.Handlers.Orders.Queries
{
    public record GetUpdateOrderViewModelQuery(int OderId) : IRequest<UpdateOrderViewModel>;
    public class GetUpdateOrderViewModelHandler : IRequestHandler<GetUpdateOrderViewModelQuery, UpdateOrderViewModel>
    {
        private readonly IOrderService _orderService;

        public GetUpdateOrderViewModelHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<UpdateOrderViewModel> Handle(GetUpdateOrderViewModelQuery request, CancellationToken cancellationToken)
        {
            return await _orderService.GetUpdateOrderViewModel(request.OderId);
        }
    }
}
