using Microsoft.AspNetCore.Mvc;

public class AppointmentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}