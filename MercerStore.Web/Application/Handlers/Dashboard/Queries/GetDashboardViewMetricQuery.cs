using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels;

namespace MercerStore.Web.Application.Handlers.Dashboard.Queries;

public record GetDashboardViewMetricQuery : IRequest<DashboardViewModel>;

public class DashboardHandler :
    IRequestHandler<GetDashboardViewMetricQuery, DashboardViewModel>
{
    private readonly IDashboardService _dashboardService;

    public DashboardHandler(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<DashboardViewModel> Handle(GetDashboardViewMetricQuery request,
        CancellationToken ct)
    {
        return await _dashboardService.GetDashboardViewMetric(ct);
    }
}
