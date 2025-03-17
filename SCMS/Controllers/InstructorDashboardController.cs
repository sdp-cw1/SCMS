using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class InstructorDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
