using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class InstructorMaterialController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
