using Microsoft.AspNetCore.Mvc;
using SCMS.Models;

namespace SCMS.Controllers
{
    public class AuthEmailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Please enter your email.";
                return View("Index");
            }

            // Check if the email exists in the database
            bool isValidEmail = new DBModel().IsValidEmail(email);

            if (isValidEmail)
            {
                // Store email in TempData for the next request
                TempData["Email"] = email;
                return RedirectToAction("Index", "AuthPassword"); // Redirect to password entry page
            }

            ViewBag.Error = "Invalid email address!";
            return View("Index");
        }
    }
}
