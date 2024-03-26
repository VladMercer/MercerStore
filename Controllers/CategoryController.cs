using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
