using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class ConfirmPasswordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ConfirmPassword(string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Error = "All fields are required!";
                return RedirectToAction("Index", "Profile");
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match!";
                return View("Index");
            }

            // Here, you can add database insertion logic for user registration
            ViewBag.Success = "Registration successful! You can now log in.";
            return RedirectToAction("Index", "CreateProfile");
        }
    }
}
