using CloudinaryDotNet.Actions;
using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Areas.Admin.ViewModels.Products;
using MercerStore.Web.Infrastructure.Data.Enum.Product;

namespace MercerStore.Web.Application.Handlers.Products.Commands;

public record CreateProductCommand(CreateProductViewModel CreateProductViewModel, int CategoryId) :
    LoggableRequest<Unit>("Manager created product", "product");

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Unit>
{
    private readonly IElasticSearchService _elasticsearchService;
    private readonly IPhotoService _photoService;
    private readonly IProductService _productService;
    private readonly ISkuService _skuService;

    public CreateProductHandler(IPhotoService photoService, IProductService productService, ISkuService skuService,
        IElasticSearchService elasticsearchService)
    {
        _photoService = photoService;
        _productService = productService;
        _skuService = skuService;
        _elasticsearchService = elasticsearchService;
    }

    public async Task<Unit> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product
        {
            CategoryId = request.CategoryId
        };

        if (request.CreateProductViewModel.MainImage != null)
        {
            var photoResult = await _photoService.AddPhotoAsync(request.CreateProductViewModel.MainImage, ct);
            MapProductDetails(product, request.CreateProductViewModel, photoResult);
        }
        else
        {
            MapProductDetails(product, request.CreateProductViewModel, null);
        }

        product.SKU = _skuService.GenerateSku(product);

        await _productService.AddProduct(product, ct);

        await _elasticsearchService.IndexProductAsync(product, ct);

        var logDetails = new
        {
            product.Name,
            product.ProductPricing.OriginalPrice,
            product.ProductDescription.DescriptionText,
            product.CategoryId,
            product.MainImageUrl,
            product.SKU,
            product.ProductStatus.Status
        };
        request.EntityId = product.Id;

        request.Details = logDetails;

        return Unit.Value;
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
