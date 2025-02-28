using MercerStore.Areas.Admin.ViewModels;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class DashboardController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public DashboardController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index()
        {
            var revenue = await _orderRepository.GetRevenue();
            var count = await _orderRepository.GetOrdersCount();
            var dashboardViewModel = new DashboardViewModel
            {
                Revenue = revenue,
                OrdersCount = count
            };
            return View(dashboardViewModel);
        }
    }
}
