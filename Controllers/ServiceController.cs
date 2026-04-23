using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PETC.Controllers
{
    public class ServiceController : Controller
    {
        // TRANG DANH SÁCH SERVICE
        public IActionResult Index()
        {
            var services = new List<dynamic>();

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Service";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    services.Add(new
                    {
                        Id = (int)reader["ServiceID"],
                        Name = reader["ServiceName"].ToString(),
                        Price = reader["Price"],
                        Description = reader["Description"].ToString()
                    });
                }
            }

            return View(services);
        }

        // TRANG CHI TIẾT SERVICE
        public IActionResult Detail(int id)
        {
            dynamic service = null;

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Service WHERE ServiceID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    service = new
                    {
                        Id = (int)reader["ServiceID"],
                        Name = reader["ServiceName"].ToString(),
                        Price = reader["Price"],
                        Description = reader["Description"].ToString(),
                        Image = reader["ImageUrl"].ToString()
                    };
                }
            }

            return View(service);
        }
        //TRANG ADMIN
        public IActionResult Admin()
        {
            var list = new List<dynamic>();

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Service";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new
                    {
                        Id = (int)reader["ServiceID"],
                        Name = reader["ServiceName"].ToString(),
                        Price = reader["Price"],
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
            dynamic service = null;

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Service WHERE ServiceID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    service = new
                    {
                        Id = (int)reader["ServiceID"],
                        Name = reader["ServiceName"].ToString(),
                        Price = reader["Price"],
                        Description = reader["Description"].ToString(),
                        ImageUrl = reader["ImageUrl"]?.ToString()
                    };
                }
            }

            return View(service);
        }
        [HttpPost]
        public IActionResult Edit(int id, string name, int price, string description, string imageUrl)
        {
            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"UPDATE Service 
                         SET ServiceName=@name, Price=@price, Description=@desc, ImageURL=@img
                         WHERE ServiceID=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@img", imageUrl);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Admin"); // 👈 QUAN TRỌNG
        }
        // Admin thêm dịch vụ
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string name, int price, string description, string imageUrl)
        {
            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"INSERT INTO Service 
                        (ServiceName, Price, Description, ImageUrl)
                        VALUES (@name, @price, @desc, @img)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@img", imageUrl);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Admin");
        }
    }
}