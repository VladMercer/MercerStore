using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers
{
	public class ProductController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
