using Microsoft.AspNetCore.Mvc;

namespace SeyahatGunlugu.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

