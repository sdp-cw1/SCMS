using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SCMS.Controllers
{
    public class LoginAfterEmailValidateController : Controller
    {
        private static readonly Dictionary<string, string> users = new Dictionary<string, string>
        {
            { "admin@example.com", "/Admin/Dashboard" },
            { "student@example.com", "/Student/Dashboard" },
            { "teacher@example.com", "/Teacher/Dashboard" }
        };

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

            if (users.TryGetValue(email, out string userDashboard))
            {
                return Redirect(userDashboard); // Redirects based on email
            }

            ViewBag.Error = "Invalid email or password!";
            return View("Index");
        }
    }
}
