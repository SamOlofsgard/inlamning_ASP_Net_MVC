using Microsoft.AspNetCore.Mvc;

namespace WebApp_Omnivus.Controllers
{
    public class Page404Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
