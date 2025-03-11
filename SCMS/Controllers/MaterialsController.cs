using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class MaterialsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
