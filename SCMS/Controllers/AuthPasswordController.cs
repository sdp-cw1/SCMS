using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class AuthPasswordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email and password are required!";
                return View("Index");
            }

            // Here, you can add database validation logic
            if (email == "user@example.com" && password == "password")
            {
                return RedirectToAction("Index", "ConfirmPassword"); // Redirect to dashboard on success
            }

            ViewBag.Error = "Invalid email or password!";
            return View("Index");
        }
    }
}
