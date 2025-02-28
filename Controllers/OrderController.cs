using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
public class OrderController : Controller
{
	private readonly IOrderRepository _orderRepository;
	private readonly IUserIdentifierService _userIdentifierService;
	private readonly ICartProductRepository _cartProductRepository;
	private readonly IUserRepository _userProfileRepository;

    public OrderController(IOrderRepository orderRepository,
        IUserIdentifierService userIdentifierService,
        ICartProductRepository cartProductRepository,
        IUserRepository userProfileRepository,
        IRequestContextService requestContextService)
    {
        _orderRepository = orderRepository;
        _userIdentifierService = userIdentifierService;
        _cartProductRepository = cartProductRepository;
        _userProfileRepository = userProfileRepository;
    }
    [HttpGet]
	public async Task<IActionResult> Index()
	{
		var userId = _userIdentifierService.GetCurrentIdentifier();
		var User = await _userProfileRepository.GetUserByIdAsyncNoTracking(userId);
		var cartViewModel = await _cartProductRepository.GetCartViewModel(userId);
		var orderViewModel = new OrderViewModel
		{
			Address = User?.Address,
			Email = User?.Email,
			CartViewModel = cartViewModel,
			PhoneNumber = User?.PhoneNumber
		};
		return View(orderViewModel);
	}

	[HttpPost]
    [LogUserAction("User created an order", "order")]
    public async Task<IActionResult> AddOrder(OrderViewModel orderViewModel)
	{
        var currentUserId = _userIdentifierService.GetCurrentIdentifier();

        if (!ModelState.IsValid)
		{
            var cartViewModel = await _cartProductRepository.GetCartViewModel(currentUserId);
			orderViewModel.CartViewModel = cartViewModel;
            return View("Index",orderViewModel);
		}

		var roles = _userIdentifierService.GetCurrentUserRoles();
		var isGuest = roles.Contains("Guest");

		var guestId = isGuest ? currentUserId : null;
		var userId = isGuest ? null : currentUserId;

		var createdOrder = await _orderRepository.CreateOrderFromCart(userId, guestId, orderViewModel);

        return RedirectToAction("Index", new { id = createdOrder.Id });
    }
}