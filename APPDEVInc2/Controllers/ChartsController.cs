using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APPDEVInc2.DataBaseModels;
using APPDEVInc2.Models;
using APPDEVInc2.Repository;
using APPDEVInc2.ViewModels.Admin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace APPDEVInc2.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Charts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetTopSellingProducts()
        {
            // Group the order details by tyre names and return
            // the tyres name with their respectful purchase count
            ApplicationDbContext context = new ApplicationDbContext();

            var query = context.OrderDetailsTbls.Include("StockTbls")
                .GroupBy(p => p.StockTbl.TyresTbl.TyreSize+" "+p.StockTbl.TyresTbl.TyreName)
                .Select(g => new { name = g.Key, count = g.Sum(w => w.Quantity) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ProductAnalysis()
        {
            return View();
        }
        public ActionResult GetMostValuedCustomers()
        {
            // Group the order details by user names and return
            // the users with the amount of purchases made
            ApplicationDbContext context = new ApplicationDbContext();
            var query = context.OrderDetailsTbls.Include("StockTbls")
               .GroupBy(p => p.OrdersTbl.UserID)
               .Select(g => new { name = g.Key, count = g.Sum(w => w.Quantity) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CustomerAnalysis()
        {
            return View();
        }

        public ActionResult MechanicCallouts()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var query = context.CalloutTbls.Include("MechanicTbls")
               .GroupBy(p => p.MechanicTbl.FirstName+" "+p.MechanicTbl.LastName)
               .Select(g => new { name = g.Key, count = g.Sum(w => w.MechanicID) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewMechanicCallouts()
        {
            return View();
        }
    }
}