using Microsoft.AspNetCore.Mvc;

namespace WebApp_Omnivus.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
