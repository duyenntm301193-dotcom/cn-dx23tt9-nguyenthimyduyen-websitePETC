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

                string query = "SELECT * FROM Doctor WHERE Status = 'Active'";
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
        // ADMIN ACTION
        public IActionResult Admin()
        {
            var list = new List<dynamic>();

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Doctor WHERE Status IS NULL OR Status = 'Active'";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new
                    {
                        Id = (int)reader["DoctorID"],
                        Name = reader["Name"].ToString(),
                        Speciality = reader["Speciality"].ToString(),
                        Experience = reader["Experience"],
                        Description = reader["Description"].ToString(),
                        ImageUrl = reader["ImageUrl"]?.ToString()
                    });
                }
            }

            return View(list);
        }
        //ADMIN CHỈNH SỬA THÔNG TIN
        public IActionResult Edit(int id)
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
                        Experience = reader["Experience"].ToString(),
                        Description = reader["Description"].ToString(),
                        ImageUrl = reader["ImageUrl"]?.ToString()
                    };
                }
            }

            return View(doctor);
        }
        // admin sửa thông tin bác sĩ
        [HttpPost]
        public IActionResult Edit(int id, string name, string speciality, string experience, string description, string imageUrl)
        {
            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"UPDATE Doctor 
                         SET Name=@name, 
                             Speciality=@speciality, 
                             Experience=@experience, 
                             Description=@description,
                             ImageUrl=@imageUrl
                         WHERE DoctorID=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@speciality", speciality);
                cmd.Parameters.AddWithValue("@experience", experience);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@imageUrl", imageUrl);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Admin"); // 👈 QUAY VỀ TRANG ADMIN
        }
        //thêm bác sĩ
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string name, string speciality, string experience, string description, string imageUrl)
        {
            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"INSERT INTO Doctor 
                        (Name, Speciality, Experience, Description, ImageUrl)
                        VALUES (@name, @spec, @exp, @desc, @img)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@spec", speciality);
                cmd.Parameters.AddWithValue("@exp", experience);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@img", imageUrl);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Admin");
        }
        //xóa bác sĩ
        public IActionResult Delete(int id)
        {
            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "UPDATE Doctor SET Status = 'Deleted' WHERE DoctorID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Admin");
        }

    }
}