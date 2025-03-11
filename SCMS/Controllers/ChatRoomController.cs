using Microsoft.AspNetCore.Mvc;

namespace SCMS.Controllers
{
    public class ChatRoomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
