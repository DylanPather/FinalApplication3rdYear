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


namespace APPDEVInc2.Controllers
{
    public class QuotationCartController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Cart
        public ActionResult Index()
        {
            var cart = QuotationCart.GetCart(this.HttpContext);
            var viewModel = new QuotationCartViewModel
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
            var addedProduct = db.StockTbls
                .Single(product => product.StockID == id);

            // Add it to the shopping cart
            var cart = QuotationCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);

            // Go back to the main Quotation Items to add
            return RedirectToAction("GenerateQuotation", "Admin");
        }

        // Increase QTY Checkout
        public ActionResult IncreaseQTY(int id)
        {
            HttpContext context = System.Web.HttpContext.Current;
            // var prods = db.TyresTbls.Single(a => a.TyreID == id);
            var x = db.StockTbls.Single(a => a.StockID == id);
            string cid = "";

            int stockid = x.StockID;

            var cart1 = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecords();
            var last = cart1.Last();
            cid = context.User.Identity.Name;

            string cartid = context.Session["QuoteCartId"].ToString();
            if (cid != null)
            {
                using (var dbs = new ApplicationDbContext())
                {
                    var cart = dbs.QuoteCarts.Where(a => a.CartID == cartid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.CartID == cartid && item.StockID == id)
                        {
                            item.Count += 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.QuoteCarts.Remove(item);
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
                    var cart = dbs.QuoteCarts.Where(a => a.CartID == cid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.CartID == cid && item.StockID == id)
                        {
                            item.Count += 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.QuoteCarts.Remove(item);
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
            var car = new ShoppingCart();
            string cid = "";
            var x = db.StockTbls.Single(a => a.StockID == id);
            int stockid = x.StockID;

            var cart1 = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecords();
            var last = cart1.Last();
            cid = context.User.Identity.Name;
            string cartid = context.Session["QuoteCartId"].ToString();

            if (cid != null)
            {
                using (var dbs = new ApplicationDbContext())
                {
                    var cart = dbs.QuoteCarts.Where(a => a.CartID == cartid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.CartID == cartid && item.StockID == id)
                        {
                            item.Count -= 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.QuoteCarts.Remove(item);
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
                    var cart = dbs.QuoteCarts.Where(a => a.CartID == cid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.CartID == cid && item.StockID == id)
                        {
                            item.Count -= 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.QuoteCarts.Remove(item);
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


            var cart = QuotationCart.GetCart(this.HttpContext);

            var viewModel = new QuotationCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
               
            };

            return View(viewModel);
        }

        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult QuoteCartSummary() 
        {
            var cart = QuotationCart.GetCart(this.HttpContext);

            ViewData["QuoteCartCount"] = cart.GetCount();
            return PartialView("QuoteCartSummary");
        }

       
        //Check if customer exists 

        public ActionResult CustomerSelection()
        {
            return View(_unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecords().ToList())
;        }


       

        public ActionResult CustomerInfo()
        {
            return View();
        }

       public ActionResult SelectCustomerForQuote(int customerid)
        {
            QuotationTbl qt = new QuotationTbl();
            string userid = this.User.Identity.Name;
            AccountController ob = new AccountController();
            using (var dbs = new ApplicationDbContext())
            {
                var cartid = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecords().ToList();
                var cd = cartid.Last();
                string cid = cd.CartID;
                var carts = dbs.QuoteCarts.Where(a => a.CartID == cid).ToList();
                foreach (var item in carts)
                {
                    item.CartID = userid;
                    dbs.SaveChanges();
                }
            };

                var cart = new QuotationCart();

            decimal? total = (from cartItems in db.QuoteCarts
                              where cartItems.CartID == userid
                              select (int?)cartItems.Count *
                              cartItems.StockTbl.Price).Sum();

            qt.CustomerID = customerid;
            qt.QuoteDate = DateTime.Now;
            qt.QuoteTotal = total;
            qt.DateModified = DateTime.Now;
            qt.IsActive = true;
            qt.IsDelete = false;
            qt.Status = "Pending";

            db.QuotationTbls.Add(qt);
            db.SaveChanges();
            return RedirectToAction("QuoteDetailPost","QuotationCart");
        }


        public ActionResult QuoteDetailPost()
        {
            QuotationDetailTbl ob = new QuotationDetailTbl();
            using (ApplicationDbContext dbs = new ApplicationDbContext())
            {

                var cart = _unitOfWork.GetRepositoryInstance<QuoteCart>().GetAllRecordsIQueryable().Where(a => a.CartID == this.User.Identity.Name);
                var getlast = db.QuotationTbls.ToArray().LastOrDefault();
                string ID = getlast.QuotationID.ToString();
                int OID = Int32.Parse(ID);
                foreach (var item in cart)
                {
                    ob.StockID = item.StockID;
                    ob.Quantity = item.Count;
                    ob.QuotationID = OID;
                    ob.Price = item.StockTbl.Price;
                    ob.VAT = item.StockTbl.Price * (decimal)0.15;
                    dbs.QuotationDetailTbls.Add(ob);
                    dbs.SaveChanges();
                }

            };
            //Posting of OrderID in HistoryTbl..

         


            using (var dbs = new ApplicationDbContext())
            {
                var cart = dbs.QuoteCarts.Where(a => a.CartID == this.User.Identity.Name).ToList();
                foreach (var item in cart)
                {
                    dbs.QuoteCarts.Remove(item);
                    dbs.SaveChanges();
                }

            };
            return RedirectToAction("ViewQuotations","Admin");

        }
       
        public ActionResult OrderDetailPost()
        {

            OrderDetailsTbl ob = new OrderDetailsTbl();
            using (ApplicationDbContext dbs = new ApplicationDbContext())
            {

                var cart = _unitOfWork.GetRepositoryInstance<Cart>().GetAllRecordsIQueryable().Where(a => a.CartID == this.User.Identity.Name);
                var getlast = db.OrdersTbls.ToArray().LastOrDefault();
                string ID = getlast.OrderID.ToString();
                int OID = Int32.Parse(ID);
                foreach (var item in cart)
                {
                    ob.StockID = item.StockID;
                    ob.Quantity = item.Count;
                    ob.OrderID = OID;
                    ob.Price = item.StockTbl.Price;
                    ob.VAT = item.StockTbl.Price * (decimal)0.15;
                    dbs.OrderDetailsTbls.Add(ob);
                    dbs.SaveChanges();
                }

            };
           

          

          

            using (var dbs = new ApplicationDbContext())
            {
                var cart = dbs.QuoteCarts.Where(a => a.CartID == this.User.Identity.Name).ToList();
                foreach (var item in cart)
                {
                    dbs.QuoteCarts.Remove(item);
                    dbs.SaveChanges();
                }

            };



            return RedirectToAction("ViewQuotations", "Admin");
        }

        public ActionResult FinalCheckOutDetail()
        {
            var cart = QuotationCart.GetCart(this.HttpContext);
            var shipid = db.HistoryTbls.ToArray().LastOrDefault();
            int shippingid = shipid.ShippingID;

            var postcode = _unitOfWork.GetRepositoryInstance<ShippingTbl>().GetAllRecordsIQueryable().Where(a => a.ShippingID == shippingid).FirstOrDefault();
            int code = postcode.PostalCode;


            var viewModel = new QuotationCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()

            };

           
            return View(viewModel);
        }

    }
}