using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc;

public class SearchController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
