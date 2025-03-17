using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
