using Microsoft.AspNetCore.Mvc;

namespace PETC.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    public IActionResult Detail(int id)
        {
            if (id == 1)
            {
                ViewBag.Name = "BS. Nguyễn Văn A";
                ViewBag.Specialty = "Chuyên khoa thú y tổng quát";
                ViewBag.Description = "Có hơn 10 năm kinh nghiệm.";
                ViewBag.Image = "https://images.unsplash.com/photo-1607746882042-944635dfe10e";
            }
            else if (id == 2)
            {
                ViewBag.Name = "BS. Trần Thị B";
                ViewBag.Specialty = "Chuyên phẫu thuật thú y";
                ViewBag.Description = "Chuyên phẫu thuật thú cưng.";
                ViewBag.Image = "https://images.unsplash.com/photo-1559839734-2b71ea197ec2";
            }

            return View();
        }
    }
}
