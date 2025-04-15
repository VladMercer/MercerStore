using MediatR;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Products.Queries
{
    public record IndexAllProductsQuery() : IRequest<Unit>;
    public class IndexAllProductsHandler : IRequestHandler<IndexAllProductsQuery, Unit>
    {
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IProductService _productService;

        public IndexAllProductsHandler(IElasticSearchService elasticSearchService, IProductService productService)
        {
            _elasticSearchService = elasticSearchService;
            _productService = productService;
        }

        public async Task<Unit> Handle(IndexAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllProducts();
            await _elasticSearchService.IndexProductsAsync(products);
            return Unit.Value;
        }
    }
}
