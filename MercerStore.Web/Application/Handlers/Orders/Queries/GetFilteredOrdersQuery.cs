using MediatR;
using MercerStore.Web.Application.Dtos.OrderDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Orders;

namespace MercerStore.Web.Application.Handlers.Orders.Queries
{
    public record GetFilteredOrdersQuery(OrderFilterRequest Request) : IRequest<PaginatedResultDto<OrderDto>>;
    public class GetFilteredOrdersHandler : IRequestHandler<GetFilteredOrdersQuery, PaginatedResultDto<OrderDto>>
    {
        private readonly IOrderService _orderService;
        public readonly IRedisCacheService _redisCacheService;
        public GetFilteredOrdersHandler(IOrderService orderService, IRedisCacheService redisCacheService)
        {
            _orderService = orderService;
            _redisCacheService = redisCacheService;
        }

        public async Task<PaginatedResultDto<OrderDto>> Handle(GetFilteredOrdersQuery query, CancellationToken cancellationToken)
        {
            var request = query.Request;

            bool isDefaultQuery =
                request.PageNumber == 1 &&
                request.PageSize == 30 &&
                !request.SortOrder.HasValue &&
                !request.Status.HasValue &&
                request.Query == "";

            string cacheKey = $"orders:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => _orderService.GetFilteredOrdersWithoutCache(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
           );
        }
    }
}