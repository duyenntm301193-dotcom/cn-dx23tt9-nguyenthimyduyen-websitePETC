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
            int userId = (int)HttpContext.Session.GetInt32("UserID");
            var services = new List<dynamic>();
            var doctors = new List<dynamic>();
            var pets = new List<dynamic>();

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
                dr.Close();

                //  PET 
                var petCmd = new SqlCommand("SELECT * FROM Pet WHERE UserID = @userId", conn);
                petCmd.Parameters.AddWithValue("@userId", userId);

                var pr = petCmd.ExecuteReader();

                while (pr.Read())
                {
                    pets.Add(new
                    {
                        Id = (int)pr["PetID"],
                        Name = pr["PetName"].ToString()
                    });
                }
                pr.Close();
            }

            ViewBag.Services = services;
            ViewBag.Doctors = doctors;
            ViewBag.Pets = pets;
            ViewBag.SelectedServiceId = serviceId;

            return View();

        }

        // ===== POST (LƯU DATABASE) =====
        [HttpPost]
        public IActionResult Index(int ServiceId, int DoctorId, int PetId, DateTime Date, string Time)
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

                string query = @"INSERT INTO Appointment 
                               (UserID, PetID,ServiceID, DoctorID, Date, Time, Status)
                        VALUES (@userId, @petId,@serviceId, @doctorId, @date, @time, 'Pending')";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@petId", PetId);
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