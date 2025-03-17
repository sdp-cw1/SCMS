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
                // Generate a random password
                var generatedPassword = new DBModel().GenerateRandomPassword();

                // Save the password to the database and send the email
                bool isSaved = new DBModel().SaveGeneratedPasswordToDB(email, generatedPassword);

                if (isSaved)
                {
                    TempData["Email"] = email;
                    return RedirectToAction("Index", "AuthPassword"); // Redirect to the AuthPassword action
                }
                else
                {
                    ViewBag.Error = "There was an issue saving the password.";
                    return View("Index");
                }
            }

            ViewBag.Error = "Invalid email address!";
            return View("Index");
        }

        public IActionResult AuthPassword()
        {
            // Retrieve the email from TempData
            var email = TempData["Email"] as string;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index" ); // Redirect to the home page if email is not found
            }

            ViewBag.Email = email;
            return View(); // Return the AuthPassword view to show the message
        }
    }
}
