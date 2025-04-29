using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Products;

namespace MercerStore.Web.Application.Handlers.Products.Queries;

public record GetProductDetailsQuery(int ProductId) : IRequest<ProductViewModel>;

public class GetProductDetailsHandler : IRequestHandler<GetProductDetailsQuery, ProductViewModel>
{
    private readonly IProductService _productService;

    public GetProductDetailsHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductViewModel> Handle(GetProductDetailsQuery request, CancellationToken ct)
    {
        return await _productService.GetProductDetails(request.ProductId, ct);
    }
}
