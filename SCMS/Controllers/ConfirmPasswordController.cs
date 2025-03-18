using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Microsoft.Extensions.Configuration;  // Ensure to add this namespace
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
            // Retrieve email from TempData to keep it for the next request
            if (TempData.ContainsKey("Email"))
            {
                ViewBag.Email = TempData["Email"];
                TempData.Keep("Email"); // Keep TempData for the next request
            }

            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult ConfirmPassword(string email, string password, string confirmPassword)
        {
            // Check if email, password, or confirm password is empty
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Error = "All fields are required!";
                return View("Index");
            }

            // Check if passwords match
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match!";
                return View("Index");
            }

            // Call the method to update the user's password in the database
            bool isPasswordUpdated = UpdateUserPassword(email, password);

            if (isPasswordUpdated)
            {
                ViewBag.Success = "Password has been successfully updated!";
                return RedirectToAction("Index", "CreateProfile"); // Redirect to CreateProfile page
            }
            else
            {
                ViewBag.Error = "There was an issue updating the password.";
                return View("Index");
            }
        }


        // Method to update the user's password securely
        private bool UpdateUserPassword(string email, string newPassword)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            string query = "UPDATE RegisteredStudents SET password = @Password WHERE email = @Email";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", newPassword);  // Use the plain password, no hashing

            try
            {
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0; // Return true if the password was successfully updated
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating password: " + ex.Message);
                return false;
            }
        }
    }
}
