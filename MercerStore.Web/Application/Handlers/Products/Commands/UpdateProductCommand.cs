using CloudinaryDotNet.Actions;
using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Areas.Admin.ViewModels.Products;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Products.Commands;

public record UpdateProductCommand(UpdateProductViewModel UpdateProductViewModel) :
    LoggableRequest<Result<Unit>>("Manager update product", "product");

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<Unit>>
{
    private readonly IDateTimeConverter _dateTimeConverter;
    private readonly IElasticSearchService _elasticsearchService;
    private readonly IPhotoService _photoService;
    private readonly IProductService _productService;

    public UpdateProductHandler(IProductService productService, IPhotoService photoService,
        IElasticSearchService elasticsearchService, IDateTimeConverter dateTimeConverter)
    {
        _productService = productService;
        _photoService = photoService;
        _elasticsearchService = elasticsearchService;
        _dateTimeConverter = dateTimeConverter;
    }

    public async Task<Result<Unit>> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var existingProduct = await _productService.GetProductById(request.UpdateProductViewModel.Id, ct);

        if (existingProduct == null) return Result<Unit>.Failure("Продукт не найден");

        Product updatedProduct;

        if (request.UpdateProductViewModel.MainImage != null)
        {
            var photoResult = await _photoService.AddPhotoAsync(request.UpdateProductViewModel.MainImage, ct);
            updatedProduct = MapViewModelToProduct(request.UpdateProductViewModel, existingProduct, photoResult);
        }
        else
        {
            updatedProduct = MapViewModelToProduct(request.UpdateProductViewModel, existingProduct, null);
        }

        await _productService.UpdateProduct(updatedProduct, ct);
        await _elasticsearchService.IndexProductAsync(updatedProduct, ct);

        var logDetails = new
        {
            updatedProduct.Id,
            updatedProduct.Name,
            updatedProduct.SKU
        };

        request.EntityId = updatedProduct.Id;
        request.Details = logDetails;

        return Result<Unit>.Success(Unit.Value);
    }

    private Product MapViewModelToProduct(UpdateProductViewModel viewModel, Product existingProduct,
        ImageUploadResult? result)
    {
        existingProduct.Name = viewModel.ProductName;
        existingProduct.MainImageUrl = result?.Url?.ToString() ?? viewModel.MainImageUrl;

        if (existingProduct.ProductPricing != null)
        {
            existingProduct.ProductPricing.OriginalPrice = viewModel.Price;
            existingProduct.ProductPricing.DiscountPercentage = viewModel.DiscountPercentage;

            if (viewModel.DiscountStart != null)
                existingProduct.ProductPricing.DiscountStart =
                    _dateTimeConverter.ConvertUserTimeToUtc(viewModel.DiscountStart.Value);

            if (viewModel.DiscountEnd != null)
                existingProduct.ProductPricing.DiscountEnd =
                    _dateTimeConverter.ConvertUserTimeToUtc(viewModel.DiscountEnd.Value);

            existingProduct.ProductPricing.DateOfPriceChange = DateTime.UtcNow;
            existingProduct.ProductPricing.FixedDiscountPrice = viewModel.DiscountPrice;
        }

        if (existingProduct.ProductDescription != null)
            existingProduct.ProductDescription.DescriptionText = viewModel.Description;

        if (existingProduct.ProductStatus != null)
        {
            existingProduct.ProductStatus.Status = viewModel.Status;
            existingProduct.ProductStatus.InStock = viewModel.InStock;
        }

        return existingProduct;
    }
}
