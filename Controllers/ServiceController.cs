using Microsoft.AspNetCore.Mvc;

namespace PETC.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            if (id == 1)
            {
                ViewBag.Name = "Khám tổng quát";
                ViewBag.Description = "Dịch vụ kiểm tra sức khỏe toàn diện cho thú cưng.";
                ViewBag.Price = "200.000 VNĐ";
                ViewBag.Image = "https://images.unsplash.com/photo-1583337130417-3346a1be7dee";
            }
            else if (id == 2)
            {
                ViewBag.Name = "Tiêm phòng";
                ViewBag.Description = "Tiêm vaccine phòng bệnh cho thú cưng.";
                ViewBag.Price = "150.000 VNĐ";
                ViewBag.Image = "https://images.unsplash.com/photo-1579154204601-01588f351e67";
            }
            else if (id == 3)
            {
                ViewBag.Name = "Phẫu thuật";
                ViewBag.Description = "Dịch vụ phẫu thuật an toàn với bác sĩ chuyên môn cao.";
                ViewBag.Price = "Tùy dịch vụ";
                ViewBag.Image = "https://images.unsplash.com/photo-1580281657527-47a67d4e9c9a";
            }

            return View();
        }
    }
}