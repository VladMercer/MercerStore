using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels;

namespace MercerStore.Web.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IOrderRepository _orderRepository;

    public DashboardService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<DashboardViewModel> GetDashboardViewMetric(CancellationToken ct)
    {
        var revenue = await _orderRepository.GetRevenue(ct);
        var count = await _orderRepository.GetOrdersCount(ct);
        return new DashboardViewModel
        {
            Revenue = revenue,
            OrdersCount = count
        };
    }
}