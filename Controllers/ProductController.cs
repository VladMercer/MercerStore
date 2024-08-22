using CloudinaryDotNet.Actions;
using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.Models.ViewModels;
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
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository,
			IPhotoService photoService, ISKUService skuService, ISKUUpdater skuUpdater)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _photoService = photoService;
            _skuService = skuService;
            _skuUpdater = skuUpdater;
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
            return View(product);
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
			_productRepository.Save();

			return RedirectToAction("CreateProduct");
		}
		[Authorize(Roles = "Admin")]
		public IActionResult UpdateSKUs()
		{
			_skuUpdater.UpdateSKUs();
			return Ok("SKUs updated successfully");
		}
	}
}