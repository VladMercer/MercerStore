using MercerStore.Web.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.ViewComponents;

public class CategoryMenuViewComponent : ViewComponent
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryMenuViewComponent(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken ct)
    {
        var categories = await _categoryRepository.GetAllCategoriesAsync(ct);
        return View(categories);
    }
}
