using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PETC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var services = new List<dynamic>();
            var doctors = new List<dynamic>();
            dynamic clinic = null;

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                //LẤY THÔNG TIN PHÒNG KHÁM
                string clinicQuery = "SELECT TOP 1 * FROM ClinicInfo";
                SqlCommand clinicCmd = new SqlCommand(clinicQuery, conn);
                SqlDataReader cr = clinicCmd.ExecuteReader();

                if (cr.Read())
                {
                    clinic = new
                    {
                        Name = cr["Name"].ToString(),
                        Description = cr["Description"].ToString(),
                        Address = cr["Address"].ToString(),
                        Phone = cr["Phone"].ToString(),
                        Email = cr["Email"].ToString(),
                        ImageUrl = cr["ImageUrl"]?.ToString()
                    };
                }
                cr.Close();

                // LẤY SERVICE 
                string serviceQuery = "SELECT * FROM Service WHERE Status IS NULL OR Status = 'Active'";
                SqlCommand serviceCmd = new SqlCommand(serviceQuery, conn);
                SqlDataReader sr = serviceCmd.ExecuteReader();

                while (sr.Read())
                {
                    services.Add(new
                    {
                        Id = (int)sr["ServiceID"],
                        Name = sr["ServiceName"].ToString(),
                        Description = sr["Description"].ToString()
                    });
                }
                sr.Close();

                // LẤY DOCTOR 
                string doctorQuery = "SELECT * FROM Doctor WHERE Status IS NULL OR Status = 'Active'";
                SqlCommand doctorCmd = new SqlCommand(doctorQuery, conn);
                SqlDataReader dr = doctorCmd.ExecuteReader();

                while (dr.Read())
                {
                    doctors.Add(new
                    {
                        Id = (int)dr["DoctorID"],
                        Name = dr["Name"].ToString(),
                        Speciality = dr["Speciality"].ToString()
                    });
                }
            }

            ViewBag.Services = services;
            ViewBag.Doctors = doctors;
            ViewBag.Clinic = clinic; 

            return View();
        }
        // ADMIN CHỈNH SỬA THÔNG TIN CLINIC
        public IActionResult Edit()
        {
        
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index");
            }

            dynamic clinic = null;

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT TOP 1 * FROM ClinicInfo";
                SqlCommand cmd = new SqlCommand(query, conn);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    clinic = new
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Address = reader["Address"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        ImageUrl = reader["ImageUrl"]?.ToString()
                    };
                }
            }

            return View(clinic);
        }
        [HttpPost]
        public IActionResult Edit(int id, string name, string description, string address, string phone, string email, string imageUrl)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index");
            }

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"UPDATE ClinicInfo
                         SET Name=@name,
                             Description=@desc,
                             Address=@addr,
                             Phone=@phone,
                             Email=@mail,
                             ImageUrl=@img
                         WHERE Id=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@addr", address);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@mail", email);
                cmd.Parameters.AddWithValue("@img", imageUrl);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}