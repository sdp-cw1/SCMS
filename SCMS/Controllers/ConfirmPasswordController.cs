using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using SCMS.Models;

namespace SCMS.Controllers
{
    public class ConfirmPasswordController : Controller
    {
        private readonly string _connectionString;

        public ConfirmPasswordController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("scmsCon");
        }

        public IActionResult Index()
        {
            if (TempData.ContainsKey("Email"))
            {
                ViewBag.Email = TempData["Email"];
                TempData.Keep("Email");
            }

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmPassword(string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Error = "All fields are required!";
                return View("Index");
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match!";
                return View("Index");
            }

            // Update password and retrieve username
            string username = UpdateUserPassword(email, password);

            if (!string.IsNullOrEmpty(username))
            {
                TempData["Email"] = email;
                TempData["Username"] = username; // Store username
                return RedirectToAction("Index", "CreateProfile");
            }
            else
            {
                ViewBag.Error = "There was an issue updating the password.";
                return View("Index");
            }
        }

        // Method to update password and fetch username
        private string UpdateUserPassword(string email, string newPassword)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string updateQuery = "UPDATE RegisteredStudents SET password = @Password WHERE email = @Email";
            using var updateCmd = new MySqlCommand(updateQuery, connection);
            updateCmd.Parameters.AddWithValue("@Email", email);
            updateCmd.Parameters.AddWithValue("@Password", newPassword);

            try
            {
                int rowsAffected = updateCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // Retrieve username after updating password
                    string fetchQuery = "SELECT username FROM RegisteredStudents WHERE email = @Email";
                    using var fetchCmd = new MySqlCommand(fetchQuery, connection);
                    fetchCmd.Parameters.AddWithValue("@Email", email);

                    var result = fetchCmd.ExecuteScalar();
                    return result != null ? result.ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating password: " + ex.Message);
            }

            return string.Empty; // Return empty if update fails
        }
    }
}
