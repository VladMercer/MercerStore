using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Services;
using MercerStore.Web.Application.ViewModels;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Mvc;
public class OrderController : Controller
{
	private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    public OrderController(IOrderService orderService, ICartService cartService)
    {
        _orderService = orderService;
        _cartService = cartService;
    }

    [HttpGet]
	public async Task<IActionResult> Index()
	{
		var orderViewModel = await _orderService.GetOrderViewModel();
		return View(orderViewModel);
	}

	[HttpPost]
    [LogUserAction("User created an order", "order")]
    public async Task<IActionResult> AddOrder(OrderViewModel orderViewModel)
	{
        if (!ModelState.IsValid)
        {
            var cartViewModel = await _cartService.GetCartViewModel();
            orderViewModel.CartViewModel = cartViewModel;
            return View("Index", orderViewModel);
        }
        var orderId = await _orderService.CreateOrderFromCart(orderViewModel);
        return RedirectToAction("Index", new { id = orderId });
    }
}