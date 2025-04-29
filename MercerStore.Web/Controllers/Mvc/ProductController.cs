using MediatR;
using MercerStore.Web.Application.Handlers.Products.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc;

[Authorize]
public class ProductController : Controller
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Details(int Id, CancellationToken ct)
    {
        var productViewModel = await _mediator.Send(new GetProductDetailsQuery(Id), ct);
        return View(productViewModel);
    }
}