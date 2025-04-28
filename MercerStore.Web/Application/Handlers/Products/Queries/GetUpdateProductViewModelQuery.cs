using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Products;

namespace MercerStore.Web.Application.Handlers.Products.Queries;

public record GetUpdateProductViewModelQuery(int ProductId) : IRequest<UpdateProductViewModel>;

public class GetUpdateProductViewModelHandler : IRequestHandler<GetUpdateProductViewModelQuery, UpdateProductViewModel>
{
    private readonly IProductService _productService;

    public GetUpdateProductViewModelHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<UpdateProductViewModel> Handle(GetUpdateProductViewModelQuery request,
        CancellationToken ct)
    {
        return await _productService.GetUpdateProductViewModel(request.ProductId, ct);
    }
}