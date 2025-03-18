using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class ConfirmPasswordController : Controller
    {
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

            // Here, you can add logic to store the user's password or proceed with further actions
            // For example: update the password in the database

            ViewBag.Success = "Password has been successfully confirmed!";
            return RedirectToAction("Index", "CreateProfile"); // Redirect to profile creation page
        }
    }
}
