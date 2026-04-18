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
    }
}