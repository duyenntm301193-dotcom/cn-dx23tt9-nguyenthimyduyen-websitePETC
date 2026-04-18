using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PETC.Controllers
{
    public class AppointmentController : Controller
    {
        string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

        // ===== GET =====
        public IActionResult Index(int? serviceId)
        {
            // 🚫 CHƯA LOGIN
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var services = new List<dynamic>();
            var doctors = new List<dynamic>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // SERVICE
                string serviceQuery = "SELECT * FROM Service";
                SqlCommand serviceCmd = new SqlCommand(serviceQuery, conn);
                SqlDataReader sr = serviceCmd.ExecuteReader();

                while (sr.Read())
                {
                    services.Add(new
                    {
                        Id = (int)sr["ServiceID"],
                        Name = sr["ServiceName"].ToString()
                    });
                }
                sr.Close();

                // DOCTOR
                string doctorQuery = "SELECT * FROM Doctor";
                SqlCommand doctorCmd = new SqlCommand(doctorQuery, conn);
                SqlDataReader dr = doctorCmd.ExecuteReader();

                while (dr.Read())
                {
                    doctors.Add(new
                    {
                        Id = (int)dr["DoctorID"],
                        Name = dr["Name"].ToString()
                    });
                }
            }

            ViewBag.Services = services;
            ViewBag.Doctors = doctors;
            ViewBag.SelectedServiceId = serviceId;

            return View();
        }

        // ===== POST (LƯU DATABASE) =====
        [HttpPost]
        public IActionResult Index(int ServiceId, int DoctorId, DateTime Date, string Time)
        {
            // 🚫 CHƯA LOGIN
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            int userId = (int)HttpContext.Session.GetInt32("UserID");

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"INSERT INTO Appointment (UserID, ServiceID, DoctorID, Date, Time)
                                 VALUES (@userId, @serviceId, @doctorId, @date, @time)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@serviceId", ServiceId);
                cmd.Parameters.AddWithValue("@doctorId", DoctorId);
                cmd.Parameters.AddWithValue("@date", Date);
                cmd.Parameters.AddWithValue("@time", Time);

                cmd.ExecuteNonQuery();
            }

            // 👉 thông báo + reload
            TempData["Success"] = "Đặt lịch thành công!";

            return RedirectToAction("Index");
        }
    }
}