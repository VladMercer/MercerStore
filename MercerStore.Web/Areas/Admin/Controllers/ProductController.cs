using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Products;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductController : Controller
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> SelectCategory()
        {
            var categories = await _productService.GetAllCategories();
            return View(categories);
        }
       
        public IActionResult SearchPage()
        {
            return View();
        }

        public IActionResult ProductPage()
        {
            return View();
        }

        [HttpGet("[area]/[controller]/create/{categoryId}")]
      
        public IActionResult CreateProduct(int categoryId)
        {
            var viewModel = new CreateProductViewModel
            {
                CategoryId = categoryId
            };
            return View(viewModel);
        }

        [HttpPost("[area]/[controller]/create/{categoryId}")]
        [LogUserAction("Manager created product", "product")]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel createViewModel, int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return View(createViewModel);
            }

            var productId = await _productService.CreateProduct(createViewModel, categoryId);
         
            return RedirectToAction("CreateProduct", new { id = productId });
        }

        public IActionResult UpdateSKUs()
        {
            _productService.UpdateSkus();
            return Ok("SKUs обновление успешно");
        }

        [HttpGet("[area]/[controller]/update/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId)
        {
            var updateProductViewModel = await _productService.GetUpdateProductViewModel(productId);
            return View(updateProductViewModel);
        }

        [HttpPost("[area]/[controller]/update/{productId}")]
        [LogUserAction("Manager update product", "product")]
        public async Task<IActionResult> UpdateProduct(UpdateProductViewModel updateProductViewModel)
        {

            var result = await _productService.UpdateProduct(updateProductViewModel);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return BadRequest();
            }
            return RedirectToAction("Index", "Dashboard", new { id = result.Data });
        }

        [HttpGet]
        public async Task<IActionResult> IndexAllProducts()
        {
            await _productService.IndexAllProducts();
            return Ok("Все продукты были проиндексированны.");
        }
     
    }
}

