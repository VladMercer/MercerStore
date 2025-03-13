using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Categories;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc
{
    [Authorize]
    public class CategoryController : Controller
    {
       
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("category/create")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCategory()
        {
            return View(new CreateCategoryViewModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("category/create")]
        [LogUserAction("Admin created category", "category")]
        public async Task<IActionResult> CreateCategory(CreateCategoryViewModel createCategoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createCategoryViewModel);
            }
            var categoryId = await _categoryService.AddCategory(createCategoryViewModel);
           
            return RedirectToAction("CreateCategory", new { id = categoryId });
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> Index(int categoryId)
        {
            var categoryPageViewModel = await _categoryService.GetCategoryPageViewModel(categoryId);
            return View(categoryPageViewModel);
        }
    }
}
