using MediatR;
using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Categories.Queries
{
    public record GetPriceRangeQuery(int CategoryId) : IRequest<PriceRangeDto>;
    public class GetPriceRangeHandler : IRequestHandler<GetPriceRangeQuery, PriceRangeDto>
    {
        private readonly ICategoryService _categoryService;

        public GetPriceRangeHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<PriceRangeDto> Handle(GetPriceRangeQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetPriceRange(request.CategoryId);
        }
    }
}
