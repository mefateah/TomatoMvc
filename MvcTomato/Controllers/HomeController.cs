using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MvcTomato.DAL;
using MvcTomato.Models;
using MvcTomato.ViewModels;
using HistoryDayViewModel = MvcTomato.ViewModels.HistoryDayViewModel;

/* 
* Implement ability saving partial data (e.g. only enter and dinner start dates)
* And show them on the next page loading (user log in)
*/

namespace MvcTomato.Controllers
{
    public class HomeController : Controller
    {
        private TomatoContext db = new TomatoContext();

        public ActionResult Index()
        {
            TimeSpan rate = TimeSpan.FromHours(7.5);
            var month = db.WorkingDays.Where(d => d.Date.Month == DateTime.Today.Month && d.Finished).ToList();
            var monthStat = month.OrderBy(d => d.Date).Select(d => rate - (d.Exit - d.Enter - (d.DinnerFinish - d.DinnerStart))).ToList();
            // Need to separate these two querys because substruction of dates is not supported in SQL
            // I convet it to List in order to force query execution (or maybe DbFunctions can be used)
            var today = db.WorkingDays.Where(d => d.Date == DateTime.Today && d.Finished).ToList();
            var todayStat = new TimeSpan?();
            if (today.Any())
            {
                todayStat = today.Select(d => rate - (d.Exit - d.Enter - (d.DinnerFinish - d.DinnerStart))).First();
            }
            ViewBag.MonthStatistics = monthStat;
            ViewBag.MonthSum = monthStat.Aggregate(TimeSpan.Zero, (TimeSpan? t1, TimeSpan? t2) => t1 + t2);
            ViewBag.MonthAllSum = month.Select(d => d.Exit - d.Enter - (d.DinnerFinish - d.DinnerStart)).Aggregate(TimeSpan.Zero, (TimeSpan? t1, TimeSpan? t2) => t1 + t2);
            ViewBag.DayStatistics = todayStat;

            var uncompleteToday = db.WorkingDays.FirstOrDefault(d => d.Date == DateTime.Today && !d.Finished);
            // TODO: Add other uncomplete days
            if (uncompleteToday != null)
            {
                ViewBag.UncompleteToday = uncompleteToday;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(WorkingDay day)
        {
            System.Diagnostics.Debug.Print("Test");
            if (ModelState.IsValid)
            {
                // TODO: Implement it using AJAX
                // Check there is no entrys in DB with that date
                if (db.WorkingDays.Any(d => d.Date == day.Date))
                {
                    return View("Error");
                }
                // TODO: for debug purpose
                if (day.Enter != null && day.Exit != null && day.DinnerStart != null && day.DinnerFinish != null)
                {
                    day.Finished = true;
                }
                db.WorkingDays.Add(day);
                db.SaveChanges();
                return View();
            }
            return View(day);
        }

        public ActionResult Edit(int id)
        {
            WorkingDay day = db.WorkingDays.Find(id);
            return View(day);
        }

        [HttpPost]
        public ActionResult Edit(WorkingDay day)
        {
            db.Entry(day).State = EntityState.Modified;
            if (day.Enter != null && day.Exit != null && day.DinnerStart != null && day.DinnerFinish != null)
            {
                day.Finished = true;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult History()
        {
            // TODO: Decide how to show uncompeted days
            var days = db.WorkingDays
                .Where(d => d.Date.Month == DateTime.Today.Month)
                .OrderBy(d => d.Date)
                .ToList()
                .Select(d => new HistoryDayViewModel(d));
            return View(days);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Delete(int id)
        {
            try
            {
                WorkingDay day = db.WorkingDays.Find(id);
                db.WorkingDays.Remove(day);
                db.SaveChanges();
            }
            catch (DataException e)
            {
                // TODO: Log error
                Console.Error.WriteLine(e.Message);
                return View("Error");
            }
            return RedirectToAction("History");
        }
    }
}