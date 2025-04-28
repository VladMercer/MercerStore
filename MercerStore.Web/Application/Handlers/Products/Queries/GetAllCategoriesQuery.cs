using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Handlers.Products.Queries;

public record GetAllCategoriesQuery : IRequest<IEnumerable<Category>>;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
{
    private readonly IProductService _productService;

    public GetAllCategoriesHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery request, CancellationToken ct)
    {
        return await _productService.GetAllCategories(ct);
    }
}