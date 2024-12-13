using Microsoft.AspNetCore.Mvc;

public class CartController : Controller
{
    public CartController() { }
    public async Task<IActionResult> Index()
    {
        return View();
    }
}
