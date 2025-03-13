using MercerStore.Web.Application.Dtos.MetricDto;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface IMetricService
    {
        Task<MetricDto> GetMetrics();
    }
}
