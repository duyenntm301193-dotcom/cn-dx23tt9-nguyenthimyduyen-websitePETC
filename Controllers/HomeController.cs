using Microsoft.AspNetCore.Mvc;

namespace PETC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}