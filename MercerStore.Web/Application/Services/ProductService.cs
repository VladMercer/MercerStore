using MercerStore.Web.Application.Dtos.ProductDto;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Products;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.ViewModels.Products;
using MercerStore.Web.Areas.Admin.ViewModels.Products;
using CloudinaryDotNet.Actions;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ISKUService _skuService;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ICategoryService _categoryService;
        private readonly ISKUUpdater _skuUpdater;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPhotoService _photoService;
        private readonly IElasticSearchService _elasticsearchService;
        private readonly IUserRepository _userProfileRepository;
        private readonly IRequestContextService _requestContextService;

        public ProductService(
            IProductRepository productRepository,
            ISKUService skuService,
            IRedisCacheService redisCacheService,
            ICategoryService categoryService,
            ISKUUpdater skuUpdater,
            ICategoryRepository categoryRepository,
            IPhotoService photoService,
            IElasticSearchService elasticsearchService,
            IUserRepository userProfileRepository,
            IRequestContextService requestContextService)
        {
            _productRepository = productRepository;
            _skuService = skuService;
            _redisCacheService = redisCacheService;
            _categoryService = categoryService;
            _skuUpdater = skuUpdater;
            _categoryRepository = categoryRepository;
            _photoService = photoService;
            _elasticsearchService = elasticsearchService;
            _userProfileRepository = userProfileRepository;
            _requestContextService = requestContextService;
        }

        public async Task<PaginatedResultDto<AdminProductDto>> GetAdminFilteredProducts(ProductFilterRequest request)
        {
            bool isDefaultQuery =
                request.CategoryId == null &&
                request.PageNumber == 1 &&
                request.PageSize == 30 &&
                !request.SortOrder.HasValue &&
                !request.Filter.HasValue;
            string cacheKey = $"products:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => FetchAdminFilteredProducts(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }
        public async Task<Product> GetProduct(int productId)
        {
            return await _productRepository.GetProductByIdAsync(productId);
        }
        public async Task<string> GetProductSku(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return _skuService.GenerateSKU(product);
        }
        private async Task<PaginatedResultDto<AdminProductDto>> FetchAdminFilteredProducts(ProductFilterRequest request)
        {
            var (products, totalItems) = await _productRepository.GetProductsAsync(request);

            var pageProducts = products
            .Select(p => new AdminProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.ProductPricing.OriginalPrice,
                MainImageUrl = p.MainImageUrl,
                CategoryId = p.CategoryId,
                Description = p.ProductDescription.DescriptionText,
                DiscountedPrice = p.ProductPricing.DiscountedPrice,
                DiscountEnd = p.ProductPricing.DiscountEnd,
                DiscountStart = p.ProductPricing.DiscountStart,
                InStock = p.ProductStatus.InStock,
                RemainingDiscountDays = p.ProductPricing.RemainingDiscountDays,
                Status = p.ProductStatus.Status switch
                {
                    ProductStatuses.Available => "В наличии",
                    ProductStatuses.OutOfStock => "Нет в наличии",
                    ProductStatuses.Archived => "Снят с продажи",
                    _ => "Неизвестный статус"
                }
            });

            var result = new PaginatedResultDto<AdminProductDto>(pageProducts, totalItems, request.PageSize);
            return result;
        }
        public async Task<ProductViewModel> GetProductDetails(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);

            string productStatus = product.ProductStatus.Status switch
            {
                ProductStatuses.Available => "В наличии",
                ProductStatuses.OutOfStock => "Нет в наличии",
                ProductStatuses.Archived => "Снят с продажи",
                _ => "Неизвестный статус"
            };

            return new ProductViewModel
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
        }
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }
        public async Task<int> CreateProduct(CreateProductViewModel createViewModel, int categoryId)
        {
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

            return product.Id;
            

        }
        public void UpdateSkus()
        {
            _skuUpdater.UpdateSKUs();
        }
        public async Task<UpdateProductViewModel> GetUpdateProductViewModel(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return new UpdateProductViewModel
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
        }
        public async Task<Result<int>> UpdateProduct(UpdateProductViewModel updateProductViewModel)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(updateProductViewModel.Id);
            if (existingProduct == null)
            {
                return Result<int>.Failure("Продукт не найден");
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

            return Result<int>.Success(updatedProduct.Id);
        }
        public async Task IndexAllProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            await _elasticsearchService.IndexProductsAsync(products);
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
            if (product == null || viewModel == null)
                throw new ArgumentNullException(nameof(product), "Product or ViewModel cannot be null.");
            product.Name = viewModel.Name;
            product.ProductPricing = new ProductPricing
            {
                OriginalPrice = viewModel.Price
            };
            product.ProductDescription = new ProductDescription
            {
                DescriptionText = viewModel.Description
            };
            product.MainImageUrl = photoResult?.Url?.ToString() ?? string.Empty;
            product.ProductStatus = new ProductStatus
            {
                IsNew = true,
                InStock = viewModel.InStock,
                Status = ProductStatuses.Available
            };
        }
    }
}
