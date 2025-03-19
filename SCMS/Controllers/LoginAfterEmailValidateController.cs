using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

                // You can fetch more user data based on the email here if needed
                ViewBag.Email = email;
                return View();  // Show a page for the next step in login
            }
        


        [HttpPost]
        public IActionResult Authenticate(string email, string password)
        {



      


            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email and password are required!";
                return View("Index");
            }

           // if (users.TryGetValue(email, out string userDashboard))
         //   {
//return Redirect(userDashboard); // Redirects based on email
         //   }


          //  bool isValidEmail = new DBModel().IsValidEmail(email);


            bool IsValidUser = new DBModel().IsValidUser(email,password);

            if (IsValidUser)
            {
                return RedirectToAction("Index", "Home"); // Redirect to Dashboard
            }


            ViewBag.Error = "Invalid email or password!";
            return View("Index");
        }
    }
}
