using APPDEVInc2.Models;
using APPDEVInc2.ViewModels.Customer;
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
using QRCoder;

namespace APPDEVInc2.Controllers
{
    public class ShoppingCartController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Cart
        public ActionResult Index()
        { 
             var cart = ShoppingCart.GetCart(this.HttpContext);
             var viewModel = new ShoppingCartViewModel
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
            var addedProduct = db.TyresTbls
                .Single(product => product.TyreID == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index", "Customer");
        }

        // Increase QTY Checkout
        public ActionResult IncreaseQTY(int id)
        {
            HttpContext context = System.Web.HttpContext.Current;
            // var prods = db.TyresTbls.Single(a => a.TyreID == id);
            var x = db.StockTbls.Single(a => a.StockID == id);
            string cid = "";
          
            int stockid = x.StockID;

            var cart1 = _unitOfWork.GetRepositoryInstance<Cart>().GetAllRecords();
            var last = cart1.Last();
            cid = context.User.Identity.Name;

            string cartid = context.Session["CartId"].ToString();
            if (cid != null)
            {
                using (var dbs = new ApplicationDbContext())
                {
                    var cart = dbs.Carts.Where(a => a.CartID == cartid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.CartID == cartid && item.StockID == id)
                        {
                            item.Count += 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.Carts.Remove(item);
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
                    var cart = dbs.Carts.Where(a => a.CartID == cid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.CartID == cid && item.StockID == id)
                        {
                            item.Count += 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.Carts.Remove(item);
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
            
            var cart1 = _unitOfWork.GetRepositoryInstance<Cart>().GetAllRecords();
            var last = cart1.Last();
            cid = context.User.Identity.Name;
            string cartid = context.Session["CartId"].ToString();

            if (cid != null)
            {
                using (var dbs = new ApplicationDbContext())
                {
                    var cart = dbs.Carts.Where(a => a.CartID == cartid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.CartID == cartid && item.StockID == id)
                        {
                            item.Count -= 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.Carts.Remove(item);
                                dbs.SaveChanges();
                            }
                        }
                    }
                };
            }
            if (cid == null) { 
                using (var dbs = new ApplicationDbContext())
                {
                    var cart = dbs.Carts.Where(a => a.CartID == cid).ToList();
                    foreach (var item in cart)
                    {
                        if (item.CartID == cid && item.StockID == id)
                        {
                            item.Count -= 1;
                            dbs.SaveChanges();
                            if (item.Count == 0)
                            {
                                dbs.Carts.Remove(item);
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


            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(),
                DeliveryCost = Decimal.Truncate(cart.GetTotal() * (decimal)0.05)
            };
          
            return View(viewModel);
        }
        
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }

        public ActionResult AddressSelection()
        {
            string userid = this.User.Identity.Name;

            var shippingdetail = _unitOfWork.GetRepositoryInstance<ShippingTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == userid).ToList();



            return View(shippingdetail);
        }

        [HttpPost]
        public ActionResult AddressSelection(FormCollection frm)
        {
            string shippingid = frm["ID"].ToString();
            // Session["TestID"] = shippingid;
            HistoryTbl ob = new HistoryTbl();
            ob.ShippingID = Int32.Parse(shippingid);
            ob.UserID = this.User.Identity.Name;
            db.HistoryTbls.Add(ob);
            db.SaveChanges();
            var customers = _unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecords().ToList();
            // var cust = _unitOfWork.GetRepositoryInstance<CustomerTbl>().GetFirstorDefaultByParameter(a => a.UserID == this.User.Identity.Name);
            // var customer = cust.UserID;


            using (ApplicationDbContext dbs = new ApplicationDbContext())
            {
                var cust = dbs.CustomerTbls.FirstOrDefault(a => a.UserID == this.User.Identity.Name);

                if (cust == null)
                {
                    return RedirectToAction("CustomerInfo", "Shoppingcart");
                }
                else
                {

                    return RedirectToAction("FinalCheckOutDetail","ShoppingCart");
                }
            };

                
               
        }

        public ActionResult CustomerInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerInfo(CustomerTbl customerTbl)
        {
            customerTbl.UserID = this.User.Identity.Name;
            customerTbl.EmailAddress = this.User.Identity.Name;
            db.CustomerTbls.Add(customerTbl);
            db.SaveChanges();

            return RedirectToAction("FinalCheckOutDetail","ShoppingCart");
        }

        public ActionResult AddNewAddress()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewAddress(ShippingTbl shipping)
        {
            string userid = this.User.Identity.Name;
            shipping.UserID = userid;
            db.ShippingTbls.Add(shipping);
            db.SaveChanges();
            return RedirectToAction("AddressSelection");
        }
        public ActionResult OrderPost()
        {
            OrdersTbl ob = new OrdersTbl();
            var cart = new ShoppingCart();
            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartID == this.User.Identity.Name
                              select (int?)cartItems.Count *
                              cartItems.StockTbl.Price).Sum();

            //Add deliverycost calculation...
            var shipid = db.HistoryTbls.ToArray().LastOrDefault();
            int shippingid = shipid.ShippingID;

            var postcode = _unitOfWork.GetRepositoryInstance<ShippingTbl>().GetAllRecordsIQueryable().Where(a => a.ShippingID == shippingid).FirstOrDefault();
            int code = postcode.PostalCode;
            if (code > 0001 && code < 4068)
            {
                ob.DeliveryCost = 150;
            }
            if (code == 4068)
            {
                ob.DeliveryCost = 0;
            }
            if (code > 4068 && code < 8889)
            {
                ob.DeliveryCost = 250;
            }


            var custid = _unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecordsIQueryable().Where(a => a.EmailAddress == this.User.Identity.Name).ToList();
            int customerid = 0;
            foreach (var item in custid)
            {
                customerid = item.CustomerID;
            }

            ob.TotalPrice = total;
            ob.DateOfOrder = DateTime.Now;
            ob.Is_Active = true;
            ob.Is_Delete = false;
            ob.Status = "Dispatch Pending";
            ob.UserID = this.User.Identity.Name;
            ob.VAT = total * (decimal)0.15;
            ob.CustomerID = customerid;
          

                db.OrdersTbls.Add(ob);
            db.SaveChanges();

            return RedirectToAction("OrderDetailPost");
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
            //Posting of OrderID in HistoryTbl..

            using (var dbs = new ApplicationDbContext())
            {
                var getlast = db.OrdersTbls.ToArray().LastOrDefault();
                string ID = getlast.OrderID.ToString();
                int OID = Int32.Parse(ID);

                var history = dbs.HistoryTbls.ToArray().LastOrDefault();
                history.OrderID = OID;

                dbs.SaveChanges();
            };

            using (var dbs = new ApplicationDbContext())
            {
                var cart = dbs.Carts.Where(a => a.CartID == this.User.Identity.Name).ToList();
                foreach (var item in cart)
                {
                    item.StockTbl.Quantity -= item.Count;
                    dbs.SaveChanges();
                    

                }
            }

            using (var dbs = new ApplicationDbContext())
            {
                var cart = dbs.Carts.Where(a => a.CartID == this.User.Identity.Name).ToList();
                foreach (var item in cart)
                {
                    dbs.Carts.Remove(item);
                    dbs.SaveChanges();
                }

            };



                return RedirectToAction("DriverAssign","ShoppingCart");
        }

        public ActionResult FinalCheckOutDetail()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var shipid = db.HistoryTbls.ToArray().LastOrDefault();
            int shippingid = shipid.ShippingID;

            var postcode = _unitOfWork.GetRepositoryInstance<ShippingTbl>().GetAllRecordsIQueryable().Where(a => a.ShippingID == shippingid).FirstOrDefault();
            int code = postcode.PostalCode;

          
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
                
            };

            if (code > 0001 && code < 4068)
            {
                viewModel.DeliveryCost = 150;
            }
            if (code == 4068)
            {
                viewModel.DeliveryCost = 0;
            }
            if(code > 4068 && code < 8889)
            {
                viewModel.DeliveryCost = 250;
            }
            return View(viewModel);
        }

        public ActionResult DriverAssign()
        {
            Random drivid = new Random();
            PayFastShipping ob = new PayFastShipping();
            var driver = _unitOfWork.GetRepositoryInstance<Drivers>().GetAllRecordsIQueryable().Where(a => a.IsAvailable == true).ToList();
            //Checks and counts the deliveries per driver that is available
          

            var driverarray = _unitOfWork.GetRepositoryInstance<Drivers>().GetAllRecordsIQueryable().Where(a => a.IsAvailable == true).ToArray();
            var notdelivered = _unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.Is_Delivered == false).ToList();
            int notdeliveredcount = notdelivered.Count();

            int[] driveramount = new int[driverarray.Count()];
            int c = 0;
            if (driverarray.Count() == 0)
            {
                ob.Status = "Unassigned";
            }
            //Counts the amount of deliveries each driver has and saves to Array

            foreach (var item in notdelivered)
            {
                for (int i=0; i < driverarray.Count(); i++)
                {
                    if (item.DriverID == driverarray[i].DriverID)
                    {
                        driveramount[i] += 1;
                    }
                }
            }

            for (int i = 0; i < driverarray.Count(); i++)
            {
                if (driveramount[i] < 3)
                {
                    ob.DriverID = driverarray[i].DriverID;
                    ob.Status = "Pending Dispatch";
                }
                else
                {
                    ob.Status = "Unassigned";
                }
            }

            var hist = db.HistoryTbls.ToArray().LastOrDefault();
            ob.OrderID = hist.OrderID;
            ob.ShippingID = hist.ShippingID;
            ob.Is_Delivered = false;
            ob.DeliveryNoteSigned = false;

            ///DateTime dt1 = new DateTime();
            ob.IsPickedUp = false;

            //QR code generation!!! ,
            string link = "https://2021grp05.azurewebsites.net/Driver/CollectOrder/" + hist.OrderID;
            //GeneratedBarcode MyBarCode = QRCodeWriter.CreateQrCode(link, 500 );
            QRCodeGenerator qr = new QRCodeGenerator();
            QRCodeData data = qr.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
            QRCode code = new QRCode(data);
            Bitmap qrCodeImage = code.GetGraphic(20);

            ImageConverter converter = new ImageConverter();
            var bytes = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));


            ob.DispatchSignImage = bytes;
           

               
               

            db.PayFastShippings.Add(ob);
            db.SaveChanges();



            return RedirectToAction("ViewOrders", "Customer");
        }

       
    }
}
