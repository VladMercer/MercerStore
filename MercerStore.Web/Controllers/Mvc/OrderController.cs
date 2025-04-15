using MediatR;
using MercerStore.Web.Application.Handlers.Orders.Commands;
using MercerStore.Web.Application.Handlers.Orders.Queries;
using MercerStore.Web.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
public class OrderController : Controller
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var orderViewModel = await _mediator.Send(new GetOrderViewModelQuery());
        return View(orderViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder(OrderViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var orderViewModel = await _mediator.Send(new GetOrderViewModelQuery());
            return View("Index", orderViewModel);
        }
        await _mediator.Send(new CreateOrderFromCartCommand(viewModel));

        return RedirectToAction("Index");
    }
}