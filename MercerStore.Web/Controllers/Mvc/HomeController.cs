using MediatR;
using MercerStore.Web.Application.Handlers.Home.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc;

public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var homePageViewModel = await _mediator.Send(new GetHomePageProductQuery(), ct);
        return View(homePageViewModel);
    }
}