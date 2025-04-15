using MediatR;
using MercerStore.Web.Application.Handlers.Orders.Commands;
using MercerStore.Web.Application.Handlers.Orders.Queries;
using MercerStore.Web.Areas.Admin.ViewModels.Orders;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
public class OrderController : Controller
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("[area]/[controller]/update/{orderId}")]
    public async Task<IActionResult> Details(int orderId)
    {
        var orderUpdateViewModel = await _mediator.Send(new GetUpdateOrderViewModelQuery(orderId));
        return View(orderUpdateViewModel);
    }

    [HttpPost("[area]/[controller]/update/")]
    public async Task<IActionResult> Details(UpdateOrderViewModel updateOrderViewModel)
    {
        if (!ModelState.IsValid) return View(updateOrderViewModel);

        await _mediator.Send(new UpdateOderCommand(updateOrderViewModel));

        return RedirectToAction("Index");
    }
}