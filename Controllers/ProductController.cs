using CloudinaryDotNet.Actions;
using MercerStore.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.Models.ViewModels;
using MercerStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ISKUUpdater _skuUpdater;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPhotoService _photoService;
        private readonly ISKUService _skuService;
        private readonly IElasticSearchService _elasticsearchService;
        private readonly IUserProfileRepository _userProfileRepository;
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository,
            IPhotoService photoService, ISKUService skuService, ISKUUpdater skuUpdater,
            IElasticSearchService elasticsearchService, IUserProfileRepository userProfileRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _photoService = photoService;
            _skuService = skuService;
            _skuUpdater = skuUpdater;
            _elasticsearchService = elasticsearchService;
            _userProfileRepository = userProfileRepository;
        }
        public async Task<IActionResult> Details(int id)
        {
            
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
            var reviews = await _productRepository.GetAllReview(id);

            var productViewModel = new ProductViewModel
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                SKU = product.SKU,
                MainImageUrl = product.MainImageUrl,
                Description = product.Description,
                Category = product.Category,
                CategoryId = product.CategoryId,
                Reviews = reviews
            };

            var createReviewViewModel = new CreateReviewViewModel
            {
                productId = product.Id
            };

            var productDetailsViewModel = new ProductDetailsViewModel
            {
                productViewModel = productViewModel,
                createReviewViewModel = createReviewViewModel
            };

            return View(productDetailsViewModel);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Details(ProductDetailsViewModel  productDetailsViewModel)
        {
            var userId = HttpContext.User.GetUserId();
            
            var review = new Review
            {
                ProductId = productDetailsViewModel.createReviewViewModel.productId,
                UserId = userId,
                Value = productDetailsViewModel.createReviewViewModel.Value,
                ReviewText = productDetailsViewModel.createReviewViewModel.ReviewText,
                Date = DateTime.UtcNow,
            };
            _productRepository.AddReview(review);
            var product = await _productRepository.GetProductByIdAsync(productDetailsViewModel.createReviewViewModel.productId);
            if (product == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { id = product.Id });


        }
        private void MapProductDetails(Product product, CreateProductViewModel viewModel, ImageUploadResult photoResult)
        {
            product.Id = viewModel.Id;
            product.Name = viewModel.Name;
            product.Price = viewModel.Price;
            product.Description = viewModel.Description;
            product.MainImageUrl = photoResult.Url.ToString();
            product.CategoryId = viewModel.CategoryId;
            //product.SKU = _skuService.GenerateSKU(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct()
        {
            var viewModel = new CreateProductViewModel
            {
                Categories = await _categoryRepository.GetAllCategoriesAsync()
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = (await _categoryRepository.GetAllCategoriesAsync()).ToList();
                return View(viewModel);
            }

            var product = new Product();

            if (viewModel.MainImage != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(viewModel.MainImage);
                MapProductDetails(product, viewModel, photoResult);
            }
            else
            {
                MapProductDetails(product, viewModel, null);
            }

            _productRepository.AddProduct(product);
            _elasticsearchService.IndexProductAsync(product);
            _productRepository.Save();

            return RedirectToAction("CreateProduct");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateSKUs()
        {
            _skuUpdater.UpdateSKUs();
            return Ok("SKUs обновление успешно");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> IndexAllProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            await _elasticsearchService.IndexProductsAsync(products);
            return Ok("Все продукты были проиндексированны.");
        }

    }
}