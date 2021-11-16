using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APPDEVInc2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard","Admin");
            }
            if (User.IsInRole("Driver"))
            {
                return RedirectToAction("Dashboard", "Driver");
            }
            if (User.IsInRole("Mechanic"))
            {
                return RedirectToAction("Dashboard", "Mechanic");
            }
            return View();
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
        public ActionResult Services()
        {
            return View();
        }
    }
}