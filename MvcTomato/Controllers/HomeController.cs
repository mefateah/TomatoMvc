using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using MvcTomato.DAL;
using MvcTomato.Models;
using HistoryDay = MvcTomato.ViewModels.HistoryDay;
using Microsoft.AspNet.Identity;
using MvcTomato.Utils;
using MvcTomato.ViewModels;

/* 
* Implement ability saving partial data (e.g. only enter and dinner start dates)
* And show them on the next page loading (user log in)
* TODO: add logging NLog + interface
* TODO: unit tests for controllers (moq, IoC)
* TODO: add all js libs using npm (jquery, bootstrap etc.) not Nuget
*/

namespace MvcTomato.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private TomatoContext db = new TomatoContext();

        public ActionResult Index()
        {
            TimeSpan rate = TimeSpan.FromHours(8);
            string userId = User.Identity.GetUserId();
            var month = db.WorkingDays
                .Where(d => d.OwnerId == userId)
                .Where(d => d.Date.Month == DateTime.Today.Month && d.Finished)
                .ToList();

            var monthStat = month.OrderBy(d => d.Date).Select(d => rate - CalculateWorkingTime(d)).ToList();
            // Need to separate these two querys because substruction of dates is not supported in SQL
            // I convet it to List in order to force query execution (or maybe DbFunctions can be used)
            var today = db.WorkingDays
                .Where(d => d.OwnerId == userId)
                .Where(d => d.Date == DateTime.Today && d.Finished)
                .ToList();

            var todayStat = new TimeSpan?();
            if (today.Any())
            {
                todayStat = today.Select(d => rate - CalculateWorkingTime(d)).First();
            }
            ViewBag.MonthStatistics = monthStat;
            var monthSum = monthStat.Aggregate(TimeSpan.Zero, (TimeSpan? t1, TimeSpan? t2) => t1 + t2);
            var monthAllSum = month.Select(CalculateWorkingTime).Aggregate(TimeSpan.Zero, (TimeSpan? t1, TimeSpan? t2) => t1 + t2);

            var uncompleted = db.WorkingDays
                .Where(d => d.OwnerId == userId && d.Finished == false).ToList();
            // TODO: Use ViewModel instead of ViewBag etc.
            // 
            var stats = new StatisticsViewModel()
            {
                UncompletedDays = ModelConverter.ToViewModels(uncompleted),
                MonthStatistic = monthSum,
                MonthSum = monthAllSum,
                TodayStatistic = todayStat
            };
            return View(stats);
        }

        private TimeSpan? CalculateWorkingTime(WorkingDay day)
        {
            var dinner = day.DinnerFinish - day.DinnerStart;
            var payedDinnerTime = TimeSpan.FromMinutes(30);
            if (dinner <= payedDinnerTime)
            {
                dinner = TimeSpan.Zero;
            }
            else
            {
                dinner = dinner - payedDinnerTime;
            }
            return (day.Exit - day.Enter) - dinner;
        }

        [HttpPost]
        public ActionResult Index(StatisticsViewModel dayWithStat)
        {
            string userId = User.Identity.GetUserId();
            var dayModel = ModelConverter.ToModel(dayWithStat.Day);
            dayModel.OwnerId = userId;
            if (ModelState.IsValid)
            {
                // TODO: Implement it using AJAX
                // Check there is no entrys in DB with the same date
                if (db.WorkingDays.Where(d => d.OwnerId == userId).Any(d => d.Date == dayModel.Date))
                {
                    ModelState.AddModelError("Date", "You already have a record with the same date");
                    return View(dayWithStat);
                }
                // TODO: for debug purpose
                if (dayWithStat.Day.Enter == null && dayWithStat.Day.Exit == null && dayWithStat.Day.DinnerStart == null && dayWithStat.Day.DinnerFinish == null)
                {
                    ModelState.AddModelError("", "At least one of Enter, Exit, Dinner Start or Dinner Finish fields should be specified");
                    return View(dayWithStat);
                }
                if (dayModel.Enter != null && dayModel.Exit != null && dayModel.DinnerStart != null && dayModel.DinnerFinish != null)
                {
                    dayModel.Finished = true;
                }
                db.WorkingDays.Add(dayModel);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(dayWithStat);
        }

        public ActionResult Edit(int id)
        {
            WorkingDay day = db.WorkingDays.Find(id);
            if (day.OwnerId != User.Identity.GetUserId())
            {
                return View("Error");
            }
            return View(day);
        }

        [HttpPost]
        public ActionResult Edit(WorkingDay day)
        {
            var dayModel = db.WorkingDays.Find(day.Id);
            if (dayModel.OwnerId != User.Identity.GetUserId())
            {
                return View("Error");
            }
            if (day.Enter == null && day.Exit == null && day.DinnerStart == null && day.DinnerFinish == null)
            {
                ModelState.AddModelError("", "At least one of Enter, Exit, Dinner Start or Dinner Finish fields should be specified");
                // suppress vlidation message for requiring OwnerId field
                ModelState.Remove("OwnerId");
                return View(day);
            }
            //db.Entry(day).State = EntityState.Modified;
            dayModel.Enter = day.Enter;
            dayModel.Exit = day.Exit;
            dayModel.DinnerStart = day.DinnerStart;
            dayModel.DinnerFinish = day.DinnerFinish;
            if (dayModel.Enter != null && dayModel.Exit != null && dayModel.DinnerStart != null && dayModel.DinnerFinish != null)
            {
                dayModel.Finished = true;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult History(int? year, int? month)
        {
            if (!year.HasValue)
            {
                year = DateTime.Today.Year;
            }
            if (!month.HasValue || month < 1 || month > 12)
            {
                month = DateTime.Today.Month;
            }
            string userId = User.Identity.GetUserId();
            // TODO: Decide how to show uncompeted days
            var days = db.WorkingDays
                .Where(d => d.OwnerId == userId)
                .Where(d => d.Date.Year == year && d.Date.Month == month)
                .OrderBy(d => d.Date)
                .ToList()
                .Select(d => new HistoryDay(d));
            var result = new HistoryViewModel
            {
                Date = new DateTime((int)year, (int)month, 1),
                AvailableYears = db.WorkingDays.Select(d => d.Date.Year).Distinct().ToList(),
                Days = days,
                TotalWorkedTime = new TimeSpan(days.Select(d => d.WorkedTime).Sum(i => i.HasValue ? i.Value.Ticks : 0)),
                TotalWorkedTimeWithDinner = new TimeSpan(days.Select(d => d.WorkedTimeWithDinner).Sum(i => i.HasValue ? i.Value.Ticks : 0))
            };
            return View(result);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Delete(int id)
        {
            string userId = User.Identity.GetUserId();
            WorkingDay day;
            try
            {
                day = db.WorkingDays.Find(id);
                if (day.OwnerId != userId)
                {
                    Console.Error.WriteLine($"The user '{userId}' is not allowed to delete '{day.Id}' entity");
                    return View("Error");
                }
                db.WorkingDays.Remove(day);
                db.SaveChanges();
            }
            catch (DataException e)
            {
                // TODO: Log error
                Console.Error.WriteLine(e.Message);
                return View("Error");
            }
            return RedirectToAction("History", new { year = day.Date.Year, month = day.Date.Month });
        }
    }
}