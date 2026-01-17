using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace SeyahatGunlugu.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // basit admin bilgisi (portföy için yeterli)
            if (username == "admin" && password == "1234")
            {
                HttpContext.Session.SetString("Admin", "true");
                return RedirectToAction("Index", "Gunluk");
            }

            ViewBag.Hata = "Kullanıcı adı veya şifre yanlış";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Index", "Gunluk");
        }
    }
}
