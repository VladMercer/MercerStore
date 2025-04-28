using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Handlers.Products.Queries;

public record GetProductQuery(int ProductId) : IRequest<Product>;

public class GetProductHandler : IRequestHandler<GetProductQuery, Product>
{
    private readonly IProductService _productService;

    public GetProductHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<Product> Handle(GetProductQuery request, CancellationToken ct)
    {
        return await _productService.GetProductById(request.ProductId, ct);
    }
}
