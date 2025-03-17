using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

