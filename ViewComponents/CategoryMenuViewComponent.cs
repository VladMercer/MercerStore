using MercerStore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class CategoryMenuViewComponent : ViewComponent
{
	private readonly ICategoryRepository _categoryRepository;

	public CategoryMenuViewComponent(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var categories = await _categoryRepository.GetAllCategoriesAsync();
		return View(categories);
	}
}