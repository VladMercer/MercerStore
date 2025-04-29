using MediatR;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Products.Queries;

public record GetProductSkuQuery(int ProductId) : IRequest<string?>;

public class GetProductSkuHandler : IRequestHandler<GetProductSkuQuery, string>
{
    private readonly IProductService _productService;
    private readonly ISkuService _skuService;

    public GetProductSkuHandler(ISkuService skuService, IProductService productService)
    {
        _skuService = skuService;
        _productService = productService;
    }

    public async Task<string> Handle(GetProductSkuQuery request, CancellationToken ct)
    {
        var product = await _productService.GetProductById(request.ProductId, ct);
        return _skuService.GenerateSku(product);
    }
}
