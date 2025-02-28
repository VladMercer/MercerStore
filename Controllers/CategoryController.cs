using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.Models.Products;
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
        private readonly IRequestContextService _requestContextService;
        public CategoryController(ICategoryRepository categoryRepository,
            IPhotoService photoService,
            IProductRepository productRepository,
            IRequestContextService requestContextService)
        {
            _categoryRepository = categoryRepository;
            _photoService = photoService;
            _productRepository = productRepository;
            _requestContextService = requestContextService;
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

            var logDetails = new
            {
                category.Name,
                category.Description,
            };

            _requestContextService.SetLogDetails(logDetails);
            _categoryRepository.AddCategory(category);

            return RedirectToAction("CreateCategory", new { id = category.Id });

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
