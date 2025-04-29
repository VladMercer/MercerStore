using MediatR;
using MercerStore.Web.Application.Handlers.Categories.Commands;
using MercerStore.Web.Application.Handlers.Categories.Queries;
using MercerStore.Web.Application.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc;

[Authorize]
public class CategoryController : Controller
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("category/create")]
    [Authorize(Roles = "Admin")]
    public IActionResult CreateCategory()
    {
        return View(new CreateCategoryViewModel());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("category/create")]
    public async Task<IActionResult> CreateCategory(CreateCategoryViewModel createCategoryViewModel,
        CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(createCategoryViewModel);

        await _mediator.Send(new AddCategoryCommand(createCategoryViewModel), ct);

        return RedirectToAction("CreateCategory");
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> Index(int categoryId, CancellationToken ct)
    {
        var categoryPageViewModel = await _mediator.Send(new GetCategoryPageViewModelQuery(categoryId), ct);
        return View(categoryPageViewModel);
    }
}