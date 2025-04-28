using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Application.ViewModels.Categories;

namespace MercerStore.Web.Application.Handlers.Categories.Commands;

public record AddCategoryCommand(CreateCategoryViewModel CreateCategoryViewModel) :
    LoggableRequest<Unit>("Manager add new category", "Category");

public class AddCategoryHandler : IRequestHandler<AddCategoryCommand, Unit>
{
    private readonly ICategoryService _categoryService;
    private readonly IPhotoService _photoService;

    public AddCategoryHandler(IPhotoService photoService, ICategoryService categoryService)
    {
        _photoService = photoService;
        _categoryService = categoryService;
    }

    public async Task<Unit> Handle(AddCategoryCommand request, CancellationToken ct)
    {
        var categoryImgUrl = "";

        if (request.CreateCategoryViewModel.CategoryImage != null)
        {
            var photoResult = await _photoService.AddPhotoAsync(request.CreateCategoryViewModel.CategoryImage, ct);
            categoryImgUrl = photoResult.Url.ToString();
        }

        var category = await _categoryService.AddCategory(request.CreateCategoryViewModel, categoryImgUrl, ct);

        request.EntityId = category.Id;
        request.Details = new { category.Name, categoryImgUrl, category.Description };

        return Unit.Value;
    }
}