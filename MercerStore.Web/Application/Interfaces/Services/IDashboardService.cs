using MercerStore.Web.Areas.Admin.ViewModels;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardViewMetric(CancellationToken ct);
}
