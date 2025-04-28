using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;

namespace MercerStore.Web.Application.Handlers.Categories.Queries;

public record GetCategoriesQuery : IRequest<IEnumerable<Category>>;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
{
    private readonly ICategoryService _categoryService;

    public GetCategoriesHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        return await _categoryService.GetAllCategories(ct);
    }
}
