using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PETC.Controllers
{
    public class PostController : Controller
    {
        string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

        // USER XEM
        public IActionResult Index()
        {
            var list = new List<dynamic>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Post WHERE Status = 'Active'";
                SqlCommand cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new
                    {
                        Id = (int)reader["PostID"],
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        ImageUrl = reader["ImageUrl"]?.ToString()
                    });
                }
            }

            return View(list);
        }
        // Admin thêm tin tức
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string title, string content, string imageUrl)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"INSERT INTO Post (Title, Content, ImageUrl)
                         VALUES (@title, @content, @img)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@content", content);
                cmd.Parameters.AddWithValue("@img", imageUrl);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Admin");
        }
        // Trang bài viết chi tiết 
        public IActionResult Detail(int id)
        {
            dynamic post = null;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Post WHERE PostID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    post = new
                    {
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        ImageUrl = reader["ImageUrl"]?.ToString()
                    };
                }
            }

            return View(post);
        }
        // TRang Admin hiển thị 
        public IActionResult Admin()
        {
            var list = new List<dynamic>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Post WHERE Status = 'Active'";
                SqlCommand cmd = new SqlCommand(query, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new
                    {
                        Id = (int)reader["PostID"],
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        ImageUrl = reader["ImageUrl"]?.ToString()
                    });
                }
            }

            return View(list);
        }
        // Admin Sửa bài viết
        public IActionResult Edit(int id)
        {
            dynamic post = null;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM Post WHERE PostID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    post = new
                    {
                        Id = (int)reader["PostID"],
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        ImageUrl = reader["ImageUrl"]?.ToString()
                    };
                }
            }

            return View(post);
        }
        [HttpPost]
        public IActionResult Edit(int id, string title, string content, string imageUrl)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"UPDATE Post 
                         SET Title=@title, Content=@content, ImageUrl=@img
                         WHERE PostID=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@content", content);
                cmd.Parameters.AddWithValue("@img", imageUrl);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Admin");
        }

        // Admin xóa bài
        public IActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "UPDATE Post SET Status='Deleted' WHERE PostID=@id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Admin");
        }
    }
}
