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

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // LẤY SERVICE
                string serviceQuery = "SELECT * FROM Service";
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
                string doctorQuery = "SELECT * FROM Doctor";
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

            return View();
        }
    }
}