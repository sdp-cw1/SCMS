using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class AuthPasswordController : Controller
    {
        public IActionResult Index()
        {
            // Retrieve the stored email and make it persist for another request
            if (TempData.ContainsKey("Email"))
            {
                ViewBag.Email = TempData["Email"];
                TempData.Keep("Email"); // Keep TempData for next request
            }
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Email is required!";
                return View("Index");
            }

            TempData["Email"] = email; // Keep email for further use

            if (string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Password is required!";
                return View("Index");
            }

            // Example authentication logic
            if (email == "user@example.com" && password == "password")
            {
                return RedirectToAction("Index", "ConfirmPassword");
            }

            ViewBag.Error = "Invalid email or password!";
            return View("Index");
        }
    }
}
