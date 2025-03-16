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
          //  if (!string.IsNullOrEmpty(email))
            //{
              //  return RedirectToAction("Index", "LoginAfterEmailValidate"); // Redirecting correctly
           // }


            bool isValidEmail = new DBModel().IsValidEmail(email);

            if (isValidEmail)
            {
                return RedirectToAction("Index", "AuthPassword"); // Redirect to Dashboard
            }
            ViewBag.Error = "Invalid login details";
            return View("Index");
        }
    }
}
