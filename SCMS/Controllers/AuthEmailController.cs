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

           // if (!string.IsNullOrEmpty(email))
            //{
                // Here, you can add logic to check the email in the database
            //    return RedirectToAction("Index", "AuthPassword"); // Redirect to Dashboard
             //            }

            bool isValidEmail = new DBModel().IsValidEmail(email);

            if (isValidEmail)
            {
                return RedirectToAction("Index", "AuthPassword"); // Redirect to Dashboard
            }

            ViewBag.Error = "Please enter a valid email.";
            return View("Index");
        }
    }
}
