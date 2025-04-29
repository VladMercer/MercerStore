using MediatR;
using MercerStore.Web.Application.Dtos.Order;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Orders;

namespace MercerStore.Web.Application.Handlers.Orders.Queries;

public record GetFilteredOrdersQuery(OrderFilterRequest Request) : IRequest<PaginatedResultDto<OrderDto>>;

public class GetFilteredOrdersHandler : IRequestHandler<GetFilteredOrdersQuery, PaginatedResultDto<OrderDto>>
{
    private readonly IOrderService _orderService;
    private readonly IRedisCacheService _redisCacheService;

    public GetFilteredOrdersHandler(IOrderService orderService, IRedisCacheService redisCacheService)
    {
        _orderService = orderService;
        _redisCacheService = redisCacheService;
    }

    public async Task<PaginatedResultDto<OrderDto>> Handle(GetFilteredOrdersQuery query,
        CancellationToken ct)
    {
        var request = query.Request;

        var isDefaultQuery =
            request is { PageNumber: 1, PageSize: 30, SortOrder: null, Status: null, Query: "" };

        const string cacheKey = "orders:page1";

        return await _redisCacheService.TryGetOrSetCacheAsync(
            cacheKey,
            () => _orderService.GetFilteredOrdersWithoutCache(request, ct),
            isDefaultQuery,
            TimeSpan.FromMinutes(10)
        );
    }
}
