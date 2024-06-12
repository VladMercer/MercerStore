using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;

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
        public async Task<IActionResult> Index(int categoryId, string sortOrder, int pageNumber = 1, int pageSize = 9)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);

            // Фильтрация и сортировка товаров (фейковая пока)
            switch (sortOrder)
            {
                case "name_asc":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Id);
                    break;
            }
            int totalItems = products.Count();
            // Пагинация
            var pagedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var viewModel = new CategoryPageViewModel
            {
                Category = category,
                Products = pagedProducts,
                SelectedCategoryId = categoryId,
                SortOrder = sortOrder,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            };

            return View(viewModel);
        }
    }
}
