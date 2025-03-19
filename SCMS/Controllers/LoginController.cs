using Microsoft.AspNetCore.Mvc;
using SCMS.Models;

namespace SCMS.Controllers
{
    public class LoginController : Controller
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
            bool isValidEmail = new DBModel().IsValidUserEmail(email);

            if (isValidEmail)
            {
                // You can add additional checks here if needed, like checking if the email is already verified
                // Redirect to another page after email validation
                TempData["Email"] = email;  // Store email temporarily for redirection
                return RedirectToAction("Index", "LoginAfterEmailValidate"); // Redirect to the LoginAfterEmailValidate page
            }

            ViewBag.Error = "Email is not registered.";
            return View("Index");
        }

    }
}
