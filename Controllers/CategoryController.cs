using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPhotoService _photoService;
        private readonly IProductRepository _productRepository;
        public CategoryController(ICategoryRepository categoryRepository, IPhotoService photoService, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _photoService = photoService;
            _productRepository = productRepository;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCategory()
        {
            return View(new CreateCategoryViewModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var category = new Category
            {
                Name = viewModel.Name,
                Description = viewModel.Description
            };

            if (viewModel.CategoryImage != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(viewModel.CategoryImage);
                category.CategoryImgUrl = photoResult.Url.ToString();
            }

            _categoryRepository.AddCategory(category);
            _categoryRepository.Save();

            return RedirectToAction("CreateCategory");

        }
        
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> Index(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

            var viewModel = new CategoryPageViewModel
            {
                Category = category,
                SelectedCategoryId = categoryId,
            };

            return View(viewModel);
        }
    }
}
