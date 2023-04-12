using AccountingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            return View(model);
        }

        [HttpGet]
        public IActionResult Login() 
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            return View(model);
        }
    }
}
