using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
