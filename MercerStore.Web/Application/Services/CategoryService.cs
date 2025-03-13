using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Categories;
using MercerStore.Web.Application.ViewModels.Categories;

namespace MercerStore.Web.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IPhotoService _photoService;
        private readonly IRequestContextService _requestContextService;
        public CategoryService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IRedisCacheService redisCacheService,
            IPhotoService photoService,
            IRequestContextService requestContextService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _redisCacheService = redisCacheService;
            _photoService = photoService;
            _requestContextService = requestContextService;
        }

        public async Task<PaginatedResultDto<ProductDto>> GetFilteredProducts(CateroryFilterRequest request)
        {
            var priceRange = await GetPriceRange(request.CategoryId);

            bool isDefaultQuery =
                request.PageNumber == 1 &&
                request.PageSize == 9 &&
                string.IsNullOrEmpty(request.SortOrder) &&
                request.MinPrice == priceRange.MinPrice &&
                request.MaxPrice == priceRange.MaxPrice;

            string cacheKey = $"products:{request.CategoryId}:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => FetchFilteredProductsAsync(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }
        public async Task<PriceRangeDto> GetPriceRange(int categoryId)
        {
            var products = await _categoryRepository.GetProductsByCategoryId(categoryId);
            var priceRange = new PriceRangeDto
            {
                MaxPrice = products.Max(p => p.ProductPricing.DiscountedPrice ?? p.ProductPricing.OriginalPrice),
                MinPrice = products.Min(p => p.ProductPricing.DiscountedPrice ?? p.ProductPricing.OriginalPrice)
            };

            return priceRange;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }

        private async Task<PaginatedResultDto<ProductDto>> FetchFilteredProductsAsync(CateroryFilterRequest request)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(request.CategoryId);

            if (request.MinPrice.HasValue)
            {
                products = products.Where(p => p.ProductPricing.OriginalPrice >= request.MinPrice.Value);
            }
            if (request.MaxPrice.HasValue)
            {
                products = products.Where(p => p.ProductPricing.OriginalPrice <= request.MaxPrice.Value);
            }

            products = request.SortOrder switch
            {
                "name_desc" => products.OrderByDescending(p => p.Name),
                "price_asc" => products.OrderBy(p => p.ProductPricing.OriginalPrice),
                "price_desc" => products.OrderByDescending(p => p.ProductPricing.OriginalPrice),
                _ => products.OrderBy(p => p.Name),
            };

            int totalItems = products.Count();

            var pageProducts = products
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.ProductPricing.OriginalPrice,
                    MainImageUrl = p.MainImageUrl,
                    CategoryId = p.CategoryId,
                    Description = p.ProductDescription.DescriptionText,
                    DiscountedPrice = p.ProductPricing.DiscountedPrice
                });

            return new PaginatedResultDto<ProductDto>(pageProducts, totalItems, request.PageSize);
        }
        public async Task<int> AddCategory(CreateCategoryViewModel createCategoryViewModel)
        {
            var category = new Category
            {
                Name = createCategoryViewModel.Name,
                Description = createCategoryViewModel.Description
            };

            if (createCategoryViewModel.CategoryImage != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(createCategoryViewModel.CategoryImage);
                category.CategoryImgUrl = photoResult.Url.ToString();
            }

            var logDetails = new
            {
                category.Name,
                category.Description,
            };

            _requestContextService.SetLogDetails(logDetails);
            return await _categoryRepository.AddCategory(category);
        }
        public async Task<CategoryPageViewModel> GetCategoryPageViewModel(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

            return new CategoryPageViewModel
            {
                Category = category,
                SelectedCategoryId = categoryId,
            };
        }
    }
}
