﻿using MediatR;
using MercerStore.Web.Application.Dtos.Metric;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Metrics.Queries;

public record GetMetricsQuery : IRequest<MetricDto>;

public class MetricHandler : IRequestHandler<GetMetricsQuery, MetricDto>
{
    private readonly IMetricService _metricService;

    public MetricHandler(IMetricService metricService)
    {
        _metricService = metricService;
    }

    public async Task<MetricDto> Handle(GetMetricsQuery request, CancellationToken ct)
    {
        return await _metricService.GetMetrics(ct);
    }
}
