using CloudinaryDotNet.Actions;
using MercerStore.Areas.Admin.ViewModels;
using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models.Products;
using MercerStore.Models.Products;
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
        private readonly IUserRepository _userProfileRepository;
        private readonly IRequestContextService _requestContextService;
        public ProductController(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IPhotoService photoService,
            ISKUService skuService,
            ISKUUpdater skuUpdater,
            IElasticSearchService elasticsearchService,
            IUserRepository userProfileRepository,
            IRequestContextService requestContextService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _photoService = photoService;
            _skuService = skuService;
            _skuUpdater = skuUpdater;
            _elasticsearchService = elasticsearchService;
            _userProfileRepository = userProfileRepository;
            _requestContextService = requestContextService;
        }
        public async Task<IActionResult> Details(int Id)
        {

            var product = await _productRepository.GetProductByIdAsync(Id);
            if (product == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);

            string productStatus = product.ProductStatus.Status switch
            {
                ProductStatuses.Available => "В наличии",
                ProductStatuses.OutOfStock => "Нет в наличии",
                ProductStatuses.Archived => "Снят с продажи",
                _ => "Неизвестный статус"
            };

            var productViewModel = new ProductViewModel
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.ProductPricing.OriginalPrice,
                SKU = product.SKU,
                MainImageUrl = product.MainImageUrl,
                Description = product.ProductDescription.DescriptionText,
                Category = category,
                CategoryId = product.CategoryId,
                Status = productStatus,
                DiscountPrice = product.ProductPricing.DiscountedPrice
            };

            return View(productViewModel);
        }
    }
}