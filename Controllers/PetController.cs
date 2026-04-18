using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PETC.Controllers
{
    public class PetController : Controller
    {
        string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

        // ===== DANH SÁCH PET =====
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            int userId = (int)HttpContext.Session.GetInt32("UserID");

            var pets = new List<dynamic>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Pet WHERE UserID = @userId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pets.Add(new
                    {
                        Id = (int)reader["PetID"],
                        Name = reader["PetName"].ToString(),
                        Type = reader["Type"].ToString(),
                        Age = reader["Age"]
                    });
                }
            }

            return View(pets);
        }

        // ===== ADD =====
        [HttpPost]
        public IActionResult Add(string name, string type, int age)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserID");

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"INSERT INTO Pet (PetName, Type, Age, UserID)
                                 VALUES (@petname, @type, @age, @userId)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@petname", name);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@userId", userId);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // ===== DELETE =====
        public IActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "DELETE FROM Pet WHERE PetID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}