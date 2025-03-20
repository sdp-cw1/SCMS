using Microsoft.AspNetCore.Mvc;
using SCMS.Models;

namespace SCMS.Controllers
{
    public class CalendarController : Controller
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
    }
}
