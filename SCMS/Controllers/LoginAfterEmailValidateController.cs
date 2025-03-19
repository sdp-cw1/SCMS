using Microsoft.AspNetCore.Mvc;
using SCMS.Models;

namespace SCMS.Controllers
{
    public class LoginAfterEmailValidateController : Controller
    {
        public IActionResult Index()
        {
            var email = TempData["Email"] as string;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Email = email;
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

            var dbModel = new DBModel();

            if (dbModel.IsValidateAdmin(email, password))
            {
                return RedirectToAction("Index", "AdminDashboard");
            }
            else if (dbModel.IsValidateTeacher(email, password))
            {
                return RedirectToAction("Index", "InstructorDashboard");
            }
            else if (dbModel.IsValidateStudent(email, password))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password!";
            return View("Index");
        }
    }
}
