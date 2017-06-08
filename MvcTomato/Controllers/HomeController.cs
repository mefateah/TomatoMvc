using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MvcTomato.DAL;
using MvcTomato.Models;

namespace MvcTomato.Controllers
{
    public class HomeController : Controller
    {
        private TomatoContext db = new TomatoContext();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(WorkingDay day)
        {
            System.Diagnostics.Debug.Print("Test");
            if (ModelState.IsValid)
            {
                db.WorkingDays.Add(day);
                db.SaveChanges();
                return RedirectToAction("Index");                
            }
            return View(day);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}