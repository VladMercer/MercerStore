using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc;

public class CartController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
