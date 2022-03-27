using Microsoft.AspNetCore.Mvc;

namespace WebApp_Omnivus.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
