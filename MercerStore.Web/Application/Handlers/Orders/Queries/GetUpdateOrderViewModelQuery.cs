using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Orders;

namespace MercerStore.Web.Application.Handlers.Orders.Queries;

public record GetUpdateOrderViewModelQuery(int OderId) : IRequest<UpdateOrderViewModel>;

public class GetUpdateOrderViewModelHandler : IRequestHandler<GetUpdateOrderViewModelQuery, UpdateOrderViewModel>
{
    private readonly IDateTimeConverter _dateTimeConverter;
    private readonly IOrderService _orderService;

    public GetUpdateOrderViewModelHandler(IOrderService orderService, IDateTimeConverter dateTimeConverter)
    {
        _orderService = orderService;
        _dateTimeConverter = dateTimeConverter;
    }

    public async Task<UpdateOrderViewModel> Handle(GetUpdateOrderViewModelQuery request,
        CancellationToken ct)
    {
        var updateOrderVieWModel = await _orderService.GetUpdateOrderViewModel(request.OderId, ct);
        if (updateOrderVieWModel.CompletedDate.HasValue)
            updateOrderVieWModel.CompletedDate =
                _dateTimeConverter.ConvertUtcToUserTime(updateOrderVieWModel.CompletedDate.Value);

        updateOrderVieWModel.CreateDate = _dateTimeConverter.ConvertUtcToUserTime(updateOrderVieWModel.CreateDate);

        return updateOrderVieWModel;
    }
}
