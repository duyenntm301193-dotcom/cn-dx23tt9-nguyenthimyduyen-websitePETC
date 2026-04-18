using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PETC.Controllers
{
    public class DoctorController : Controller
    {
        // DANH SÁCH
        public IActionResult Index()
        {
            var doctors = new List<dynamic>();

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Doctor";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    doctors.Add(new
                    {
                        Id = (int)reader["DoctorID"],
                        Name = reader["Name"].ToString(),
                        Speciality = reader["Speciality"].ToString(),
                        Image = reader["ImageUrl"].ToString()
                    });
                }
            }

            return View(doctors);
        }

        // DETAIL
        public IActionResult Detail(int id)
        {
            dynamic doctor = null;

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Doctor WHERE DoctorID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    doctor = new
                    {
                        Id = (int)reader["DoctorID"],
                        Name = reader["Name"].ToString(),
                        Speciality = reader["Speciality"].ToString(),
                        Experience = reader["Experience"],
                        Description = reader["Description"].ToString(),
                        Image = reader["ImageUrl"].ToString()
                    };
                }
            }

            return View(doctor);
        }
    }
}