using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Orders;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MercerStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class OrderController : Controller
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[area]/[controller]/update/{orderId}")]
        public async Task<IActionResult> Details(int orderId)
        {
            var orderUpdateViewModel = await _orderService.GetUpdateOrderViewModel(orderId);
            return View(orderUpdateViewModel);
        }

        [HttpPost("[area]/[controller]/update/")]
        [LogUserAction("Manager update order", "order")]
        public async Task<IActionResult> Details(UpdateOrderViewModel updateOrderViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateOrderViewModel);
            }

            await _orderService.UpdateOder(updateOrderViewModel);

            return RedirectToAction("Index", new { id = updateOrderViewModel.Id });
        }
    }
}
