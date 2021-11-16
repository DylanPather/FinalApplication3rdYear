using APPDEVInc2.Models;
using APPDEVInc2.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APPDEVInc2.Repository;
using APPDEVInc2.DataBaseModels;
using Microsoft.AspNet.Identity;
using System.Data;
using IronBarCode;
using System.Drawing;
using System.IO;
using APPDEVInc2.ViewModels.Mechanic;

namespace APPDEVInc2.Controllers
{
    public class CalloutReportCartController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Cart
        public ActionResult Index()
        {
            var cart = CalloutReportingCart.GetCart(this.HttpContext);
            var viewModel = new CalloutReportCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);

            // var cart = _unitOfWork.GetRepositoryInstance<Cart>().GetAllRecordsIQueryable();
            // Set up our ViewModel

            // Return the view
            //var viewModel = cart;

        }
        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedProduct = db.CalloutServices
                .Single(product => product.CalloutServiceID == id);

            // Add it to the shopping cart
            var cart = CalloutReportingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);

            // Go back to the main Quotation Items to add
            return RedirectToAction("ReportForCalloutStart", "Mechanic", new { id = Int32.Parse(Session["CalloutID"] + "") });
        }

        // Increase QTY Checkout
        public ActionResult IncreaseQTY(int id)
        {
            HttpContext context = System.Web.HttpContext.Current;
            // var prods = db.TyresTbls.Single(a => a.TyreID == id);
            var x = db.CalloutServices.Single(a => a.CalloutServiceID == id);
            string cid = "";

            int stockid = x.CalloutServiceID;

            var cart1 = _unitOfWork.GetRepositoryInstance<CalloutReportCart>().GetAllRecords();
            var last = cart1.Last();
            cid = context.User.Identity.Name;

            string cartid = context.Session["ReportCartId"].ToString();
            if (cid != null)
            {
                using (var dbs = new ApplicationDbContext())
                {
                    var cart = dbs.CalloutReportCarts.Where(a => a.VehicleID == cartid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.VehicleID == cartid && item.CalloutServiceID == id)
                        {
                            item.Count += 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.CalloutReportCarts.Remove(item);
                                dbs.SaveChanges();
                            }
                        }
                    }
                };
            }
            if (cid == null)
            {
                using (var dbs = new ApplicationDbContext())
                {
                    var cart = dbs.CalloutReportCarts.Where(a => a.VehicleID == cartid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.VehicleID == cid && item.CalloutServiceID == id)
                        {
                            item.Count += 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.CalloutReportCarts.Remove(item);
                                dbs.SaveChanges();
                            }
                        }
                    }
                };

            }


            return RedirectToAction("Index");
        }

        // Decrease QTY Checkout
        public ActionResult DecreaseQTY(int id)
        {
            HttpContext context = System.Web.HttpContext.Current;
            var car = new CalloutReport();
            string cid = "";
            var x = db.CalloutServices.Single(a => a.CalloutServiceID == id);
            int stockid = x.CalloutServiceID;

            var cart1 = _unitOfWork.GetRepositoryInstance<CalloutReportCart>().GetAllRecords();
            var last = cart1.Last();
            cid = context.User.Identity.Name;
            string cartid = context.Session["ReportCartId"].ToString();

            if (cid != null)
            {
                using (var dbs = new ApplicationDbContext())
                {
                    var cart = dbs.CalloutReportCarts.Where(a => a.VehicleID == cartid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.VehicleID == cartid && item.CalloutServiceID == id)
                        {
                            item.Count -= 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.CalloutReportCarts.Remove(item);
                                dbs.SaveChanges();
                            }
                        }
                    }
                };
            }
            if (cid == null)
            {
                using (var dbs = new ApplicationDbContext())
                {
                    var cart = dbs.CalloutReportCarts.Where(a => a.VehicleID == cid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.VehicleID == cid && item.CalloutServiceID == id)
                        {
                            item.Count -= 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.CalloutReportCarts.Remove(item);
                                dbs.SaveChanges();
                            }
                        }
                    }
                };

            }

            return RedirectToAction("Index");
        }

        public ActionResult CheckoutDetail()
        {

            string name = this.HttpContext.User.ToString();


            var cart = CalloutReportingCart.GetCart(this.HttpContext);

            var viewModel = new CalloutReportCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()

            };

            return View(viewModel);
        }

        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CalloutReportCartSummary() 
        {
            var cart = CalloutReportingCart.GetCart(this.HttpContext);

            ViewData["CalloutReportCartCount"] = cart.GetCount();
            return PartialView("CalloutReportCartSummary");
        }

    }
}
