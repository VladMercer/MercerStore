using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Categories;

namespace MercerStore.Web.Application.Handlers.Categories.Queries
{
    public record GetCategoryPageViewModelQuery(int CategoryId) : IRequest<CategoryPageViewModel>;
    public class GetCategoryPageViewModelHandler : IRequestHandler<GetCategoryPageViewModelQuery, CategoryPageViewModel>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoryPageViewModelHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<CategoryPageViewModel> Handle(GetCategoryPageViewModelQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetCategoryPageViewModel(request.CategoryId);
        }
    }
}
