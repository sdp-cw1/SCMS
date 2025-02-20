using Microsoft.AspNetCore.Mvc;

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
            if (!string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index", "LoginAfterEmailValidate"); // Redirecting correctly
            }
            ViewBag.Error = "Invalid login details";
            return View("Index");
        }
    }
}
