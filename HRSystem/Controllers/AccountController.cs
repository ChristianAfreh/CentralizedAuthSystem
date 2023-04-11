using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Register()
		{
			return View();
		}
	}
}
