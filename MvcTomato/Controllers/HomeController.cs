using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MvcTomato.DAL;
using MvcTomato.Models;
using HistoryDayViewModel = MvcTomato.ViewModels.HistoryDayViewModel;
using Microsoft.AspNet.Identity;
using MvcTomato.Utils;
using MvcTomato.ViewModels;

/* 
* Implement ability saving partial data (e.g. only enter and dinner start dates)
* And show them on the next page loading (user log in)
* TODO: add logging NLog + interface
* TODO: unit tests for controllers (moq, IoC)
*/

namespace MvcTomato.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private TomatoContext db = new TomatoContext();

        public ActionResult Index()
        {
            TimeSpan rate = TimeSpan.FromHours(7.5);
            string userId = User.Identity.GetUserId();
            var month = db.WorkingDays
                .Where(d => d.OwnerId == userId)
                .Where(d => d.Date.Month == DateTime.Today.Month && d.Finished)
                .ToList();

            var monthStat = month.OrderBy(d => d.Date).Select(d => rate - (d.Exit - d.Enter - (d.DinnerFinish - d.DinnerStart))).ToList();
            // Need to separate these two querys because substruction of dates is not supported in SQL
            // I convet it to List in order to force query execution (or maybe DbFunctions can be used)
            var today = db.WorkingDays
                .Where(d => d.OwnerId == userId)
                .Where(d => d.Date == DateTime.Today && d.Finished)
                .ToList();

            var todayStat = new TimeSpan?();
            if (today.Any())
            {
                todayStat = today.Select(d => rate - (d.Exit - d.Enter - (d.DinnerFinish - d.DinnerStart))).First();
            }
            ViewBag.MonthStatistics = monthStat;
            ViewBag.MonthSum = monthStat.Aggregate(TimeSpan.Zero, (TimeSpan? t1, TimeSpan? t2) => t1 + t2);
            ViewBag.MonthAllSum = month.Select(d => d.Exit - d.Enter - (d.DinnerFinish - d.DinnerStart)).Aggregate(TimeSpan.Zero, (TimeSpan? t1, TimeSpan? t2) => t1 + t2);
            ViewBag.DayStatistics = todayStat;

            var uncompleteToday = db.WorkingDays
                .Where(d => d.OwnerId == userId)
                .FirstOrDefault(d => d.Date == DateTime.Today && !d.Finished);
            // TODO: Add other uncomplete days
            if (uncompleteToday != null)
            {
                ViewBag.UncompleteToday = ModelConverter.ToViewModel(uncompleteToday);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(WorkingDayViewModel day)
        {
            string userId = User.Identity.GetUserId();
            var dayModel = ModelConverter.ToModel(day);
            dayModel.OwnerId = userId;
            if (ModelState.IsValid)
            {
                // TODO: Implement it using AJAX
                // Check there is no entrys in DB with that date
                if (db.WorkingDays.Where(d => d.OwnerId == userId).Any(d => d.Date == dayModel.Date))
                {
                    return View("Error");
                }
                // TODO: for debug purpose
                // TODO: disable submit button if all fields are empty
                if (dayModel.Enter != null && dayModel.Exit != null && dayModel.DinnerStart != null && dayModel.DinnerFinish != null)
                {
                    dayModel.Finished = true;
                }
                db.WorkingDays.Add(dayModel);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(day);
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

        public ActionResult History()
        {
            string userId = User.Identity.GetUserId();
            // TODO: Decide how to show uncompeted days
            var days = db.WorkingDays
                .Where(d => d.OwnerId == userId)
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
            string userId = User.Identity.GetUserId();
            try
            {
                WorkingDay day = db.WorkingDays.Find(id);
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
            return RedirectToAction("History");
        }
    }
}