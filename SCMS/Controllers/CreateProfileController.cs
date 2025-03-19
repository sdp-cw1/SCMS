using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using SCMS.Models;
using System;

namespace SCMS.Controllers
{
    public class CreateProfileController : Controller
    {
        private readonly string _connectionString;

        public CreateProfileController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("scmsCon");
        }

        public IActionResult Index()
        {
            if (TempData.ContainsKey("Username") && TempData.ContainsKey("Email"))
            {
                ViewBag.Username = TempData["Username"];
                ViewBag.Email = TempData["Email"];
                // Keep the TempData for further requests
                TempData.Keep("Username");
                TempData.Keep("Email");
            }
            else
            {
                ViewBag.Error = "User data not found. Please try again.";
            }

            return View();
        }

        [HttpPost]
        public IActionResult CreateProfile(string firstName, string lastName, string nic, string mobile, string address, string dob, bool? notifyEmail, bool? notifyApp)
        {
            string username = TempData["Username"] as string;
            string email = TempData["Email"] as string;

            // Reassign TempData so it persists
            if (!string.IsNullOrEmpty(username))
                TempData["Username"] = username;
            if (!string.IsNullOrEmpty(email))
                TempData["Email"] = email;

            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Session expired. Please log in again.";
                return View("Index");
            }

            string notifyMethod = (notifyEmail == true) ? "email" : (notifyApp == true) ? "app" : "none";

            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                // Check if email exists in RegisteredStudents table
                using var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM RegisteredStudents WHERE Email = @Email", connection);
                checkCmd.Parameters.AddWithValue("@Email", email);
                int emailExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (emailExists == 0)
                {
                    ViewBag.Error = "You must register first!";
                    return View("Index");
                }

                // Call the stored procedure to insert profile
                using var cmd = new MySqlCommand("CALL InsertUserAndStu(@Email, @Nic, @Phone, @DOB, @FirstName, @LastName, @Address, @NotifyMethod)", connection);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Nic", nic);
                cmd.Parameters.AddWithValue("@Phone", mobile);
                cmd.Parameters.AddWithValue("@DOB", dob);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@NotifyMethod", notifyMethod);

                cmd.ExecuteNonQuery();

                TempData["Success"] = "Profile created successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Database error: " + ex.Message;
                return View("Index");
            }
        }
    }
}
