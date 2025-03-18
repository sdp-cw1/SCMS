using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class CreateProfileController : Controller
    {
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Username") && TempData.ContainsKey("Email"))
            {
                ViewBag.Username = TempData["Username"];
                ViewBag.Email = TempData["Email"];
            }
            else
            {
                ViewBag.Error = "User data not found. Please try again.";
            }

            return View();
        }

        [HttpPost]
        public IActionResult CreateProfile(string firstName, string lastName, string nic, string mobile, string address, bool notifyText, bool notifyEmail, bool notifyApp)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(nic) || string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(address))
            {
                ViewBag.Error = "All fields are required!";
                return View("Index");
            }

            ViewBag.Success = "Profile created successfully!";
            return View("Index");
        }
    }
}
