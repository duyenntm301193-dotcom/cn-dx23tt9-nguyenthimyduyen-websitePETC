using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PETC.Controllers
{
    public class AuthController : Controller
    {
        string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

        // ===== REGISTER =====
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string name, string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "INSERT INTO [User] (Name, Email, Password, Role) VALUES (@name, @email, @password, 'User')";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Login");
        }

        // ===== LOGIN =====
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM [User] WHERE Email=@email AND Password=@password";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    HttpContext.Session.SetString("UserName", reader["Name"].ToString());
                    HttpContext.Session.SetInt32("UserID", (int)reader["UserID"]);
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Sai email hoặc mật khẩu";
            return View();

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}