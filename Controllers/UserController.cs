using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PETC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Admin()
        {
            var users = new List<dynamic>();
            var details = new List<dynamic>();

            string connStr = "Server=localhost\\SQLEXPRESS;Database=PETC_DB;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // SER
                var cmdUser = new SqlCommand("SELECT UserID, Name, Email FROM [User]", conn);
                var ur = cmdUser.ExecuteReader();

                while (ur.Read())
                {
                    users.Add(new
                    {
                        Id = (int)ur["UserID"],
                        Name = ur["Name"].ToString(),
                        Email = ur["Email"].ToString()
                    });
                }
                ur.Close();

                // PET + APPOINTMENT 
                string query = @"
        SELECT 
            p.UserID,
            p.PetID,
            p.PetName,
            a.Date,
            a.Time,
            a.Status
        FROM Pet p
        LEFT JOIN Appointment a ON p.PetID = a.PetID";

                var cmd = new SqlCommand(query, conn);
                var r = cmd.ExecuteReader();

                while (r.Read())
                {
                    details.Add(new
                    {
                        UserID = (int)r["UserID"],
                        PetID = (int)r["PetID"],
                        PetName = r["PetName"].ToString(),
                        Date = r["Date"]?.ToString(),
                        Time = r["Time"]?.ToString(),
                        Status = r["Status"]?.ToString()
                    });
                }
            }

            ViewBag.Users = users;
            ViewBag.Details = details;

            return View();
        }
    }
}