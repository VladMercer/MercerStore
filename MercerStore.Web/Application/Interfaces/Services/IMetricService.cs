using MercerStore.Web.Application.Dtos.Metric;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IMetricService
{
    Task<MetricDto> GetMetrics(CancellationToken ct);
}
