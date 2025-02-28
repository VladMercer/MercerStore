using CloudinaryDotNet.Actions;
using MercerStore.Areas.Admin.ViewModels;
using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductController : Controller
    {
        private readonly ISKUUpdater _skuUpdater;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPhotoService _photoService;
        private readonly ISKUService _skuService;
        private readonly IElasticSearchService _elasticsearchService;
        private readonly IUserRepository _userProfileRepository;
        private readonly IRequestContextService _requestContextService;

        public ProductController(ISKUUpdater skuUpdater,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IPhotoService photoService,
            ISKUService skuService,
            IElasticSearchService elasticsearchService,
            IRequestContextService requestContextService)
        {
            _skuUpdater = skuUpdater;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _photoService = photoService;
            _skuService = skuService;
            _elasticsearchService = elasticsearchService;
            _requestContextService = requestContextService;
        }

     
        public async Task<IActionResult> SelectCategory()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
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

            var product = new Product();

            if (createViewModel.MainImage != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(createViewModel.MainImage);
                MapProductDetails(product, createViewModel, photoResult);
            }
            else
            {
                MapProductDetails(product, createViewModel, null);
            }

            product.CategoryId = categoryId;
            await _productRepository.AddProduct(product);
            product.SKU = _skuService.GenerateSKU(product);
            await _productRepository.UpdateProduct(product);
            await _elasticsearchService.IndexProductAsync(product);

            var logDetails = new
            {
                product.Name,
                product.ProductPricing.OriginalPrice,
                product.ProductDescription.DescriptionText,
                categoryId,
                product.MainImageUrl,
                product.SKU
            };

            _requestContextService.SetLogDetails(logDetails);

            return RedirectToAction("CreateProduct", new { id = product.Id });
        }

        public IActionResult UpdateSKUs()
        {
            _skuUpdater.UpdateSKUs();
            return Ok("SKUs обновление успешно");
        }

        [HttpGet("[area]/[controller]/update/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            var updateProductViewModel = new UpdateProductViewModel
            {
                Id = product.Id,
                ProductName = product.Name,
                Price = product.ProductPricing.OriginalPrice,
                RemainingDiscountDays = product.ProductPricing.RemainingDiscountDays,
                Status = product.ProductStatus.Status,
                DiscountPercentage = product.ProductPricing.DiscountPercentage,
                DiscountEnd = product.ProductPricing.DiscountEnd,
                Description = product.ProductDescription.DescriptionText,
                InStock = product.ProductStatus.InStock,
                MainImageUrl = product.MainImageUrl,
                DiscountPrice = product.ProductPricing.FixedDiscountPrice,
                DiscountStart = product.ProductPricing.DiscountStart
            };

          
            return View(updateProductViewModel);
        }

        [HttpPost("[area]/[controller]/update/{productId}")]
        [LogUserAction("Manager update product", "product")]
        public async Task<IActionResult> UpdateProduct(UpdateProductViewModel updateProductViewModel)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(updateProductViewModel.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            Product updatedProduct;

            if (updateProductViewModel.MainImage != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(updateProductViewModel.MainImage);
                updatedProduct = MapViewModelToProduct(updateProductViewModel, existingProduct, photoResult);
            }
            else
            {
                updatedProduct = MapViewModelToProduct(updateProductViewModel, existingProduct, null);
            }

            var logDetails = new
            {
                updatedProduct.Id,
                updatedProduct.Name,
                updatedProduct.SKU
            };

            _requestContextService.SetLogDetails(logDetails);

            await _productRepository.UpdateProduct(updatedProduct);
            await _elasticsearchService.IndexProductAsync(updatedProduct);

            return RedirectToAction("Index", "Dashboard", new { id = updatedProduct.Id });
        }

        [HttpGet]
        public async Task<IActionResult> IndexAllProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            await _elasticsearchService.IndexProductsAsync(products);
            return Ok("Все продукты были проиндексированны.");
        }
        private Product MapViewModelToProduct(UpdateProductViewModel viewModel, Product existingProduct, ImageUploadResult? result)
        {
            existingProduct.Name = viewModel.ProductName;
            existingProduct.MainImageUrl = result?.Url?.ToString() ?? viewModel.MainImageUrl;

            if (existingProduct.ProductPricing != null)
            {
                existingProduct.ProductPricing.OriginalPrice = viewModel.Price;
                existingProduct.ProductPricing.DiscountPercentage = viewModel.DiscountPercentage;
                existingProduct.ProductPricing.DiscountStart = viewModel.DiscountStart;
                existingProduct.ProductPricing.DiscountEnd = viewModel.DiscountEnd;
                existingProduct.ProductPricing.DateOfPriceChange = DateTime.UtcNow;
                existingProduct.ProductPricing.FixedDiscountPrice = viewModel.DiscountPrice;
            }

            if (existingProduct.ProductDescription != null)
            {
                existingProduct.ProductDescription.DescriptionText = viewModel.Description;
            }

            if (existingProduct.ProductStatus != null)
            {
                existingProduct.ProductStatus.Status = viewModel.Status;
                existingProduct.ProductStatus.InStock = viewModel.InStock;
            }

            return existingProduct;
        }
        private void MapProductDetails(Product product, CreateProductViewModel viewModel, ImageUploadResult photoResult)
        {
            product.Name = viewModel.Name;
            product.ProductPricing = new ProductPricing
            {
                OriginalPrice = viewModel.Price
            };
            product.ProductDescription = new ProductDescription
            {
                DescriptionText = viewModel.Description
            };
            product.MainImageUrl = photoResult.Url.ToString();
            product.ProductStatus = new ProductStatus
            {
                IsNew = true,
                InStock = viewModel.InStock,
                Status = ProductStatuses.Available
            };
        }
    }
}

