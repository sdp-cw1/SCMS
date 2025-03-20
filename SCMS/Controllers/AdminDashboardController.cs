using Microsoft.AspNetCore.Mvc;
using SCMS.Models;

namespace SCMS.Controllers
{
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {

            int userId = 1; // Replace with the actual admin's user ID
            DateTime today = DateTime.Today;
            DateOnly startDate = new DateOnly(today.Year, today.Month, 1);
            DateOnly endDate = startDate.AddMonths(1).AddDays(-1);

            var db = new DBInsertSchedule();
            var schedules = db.GetScheduleList(userId, startDate, endDate);

            ViewBag.Schedules = schedules;
            ViewBag.CurrentMonthStart = startDate;
            ViewBag.DaysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            ViewBag.FirstDayOfWeek = (int)new DateTime(startDate.Year, startDate.Month, 1).DayOfWeek;
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
                string EventLocation
                // string Note
                )
        {
            DateOnly eventDt = DateOnly.Parse(EventDate);
            DateTime startDt = eventDt.ToDateTime(TimeOnly.Parse(StartTime));
            DateTime endDt = eventDt.ToDateTime(TimeOnly.Parse(EndTime));
            new DBInsertSchedule().CreateSchedule(
                    StartDateTime: startDt,
                    endDateTime: endDt,
                    Location: EventLocation,
                    eventName: EventName,
                    category: ScheduleType,
                    organiser: 1, // sample user_id
                    module: "MD0001" // sample module_id
                    );
            return RedirectToAction("Index");
        }
    }
}
