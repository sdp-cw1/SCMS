using Microsoft.AspNetCore.Mvc;
using SCMS.Models;

namespace SCMS.Controllers
{
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSchedule(
                string EventName,
                string EventDate,
                string ScheduleType,
                string TargetBatch,
                string StartTime,
                string EndTime,
                string Location,
                string Note)
        {
            DateOnly eventDt = DateOnly.Parse(EventDate);
            DateTime startDt = eventDt.ToDateTime(TimeOnly.Parse(StartTime));
            DateTime endDt = eventDt.ToDateTime(TimeOnly.Parse(EndTime));
            new DBInsertSchedule().CreateSchedule(
                    StartDateTime: startDt,
                    endDateTime: endDt,
                    Location: Location,
                    eventName: EventName,
                    category: ScheduleType,
                    organiser: 1, // sample user_id
                    module: "MD0001" // sample module_id
                    );
            return View();
        }
    }
}
