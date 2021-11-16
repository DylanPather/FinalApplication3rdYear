
using APPDEVInc2.DataBaseModels;
using APPDEVInc2.Models;
using APPDEVInc2.Repository;
using APPDEVInc2.ViewModels.Admin;
using IronBarCode;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace APPDEVInc2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private ApplicationDbContext db = new ApplicationDbContext();
        private int customerID;
        private string cartID;
        private int vehid;
        private int quoteid;
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public List<SelectListItem> GetMechanics()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecords();
            foreach (var item in cat)
            {
                list.Add(new SelectListItem { Value = item.MechanicID.ToString(), Text = item.FirstName + " " + item.LastName });
            }
            return list;
        }
        public List<SelectListItem> GetBays()
        {

            List<SelectListItem> baylist = new List<SelectListItem>();
            var cust = _unitOfWork.GetRepositoryInstance<BayTbl>().GetAllRecords().Where(a => a.IsAvailable == true && a.BayName == "Mechanics Bay");
            foreach (var item in cust)
            {
                baylist.Add(new SelectListItem { Value = item.BayID.ToString(), Text = item.BayID.ToString() });
            }
            return baylist;
        }

        public List<SelectListItem> GetTools()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var tool = _unitOfWork.GetRepositoryInstance<ToolsTbl>().GetAllRecords();
            foreach(var item in tool)
            {
                list.Add(new SelectListItem { Value = item.ToolID.ToString(), Text = item.ToolName });
            }

            return list;

        }
        public List<SelectListItem> GetDrivers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var drivers = _unitOfWork.GetRepositoryInstance<Drivers>().GetAllRecordsIQueryable().Where(a => a.IsAvailable == true).ToList();
           
            
            foreach (var item in drivers)
            {
                list.Add(new SelectListItem { Value = item.DriverID.ToString(), Text = item.FullName });
            }

            return list;

        }
      

        public ActionResult Dashboard()
        {
            return View();
        }
        public List<SelectListItem> GetCustomer()
        {
            List<SelectListItem> custlist = new List<SelectListItem>();
            var cust = _unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecords();
            foreach (var item in cust)
            {
                custlist.Add(new SelectListItem { Value = item.CustomerID.ToString(), Text = item.EmailAddress });
            }
            return custlist;
        }
        public ActionResult AddTyres()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddTyres(TyresTbl tyresTbl, HttpPostedFileBase file)
        {
            TyresTbl ob = new TyresTbl();
            if (file != null)
            {
                tyresTbl.Image = new byte[file.ContentLength];
                file.InputStream.Read(tyresTbl.Image, 0, file.ContentLength);
            }
            ob.Image = tyresTbl.Image;

            db.TyresTbls.Add(tyresTbl);



            db.SaveChanges();
            return RedirectToAction("TyreStockPost");
        }

        public ActionResult TyreStockPost()
        {


            return View();
        }

        [HttpPost]
        public ActionResult TyreStockPost(StockTbl tbl)
        {
            var tyre = _unitOfWork.GetRepositoryInstance<TyresTbl>().GetAllRecords().ToList();
            var x = tyre.LastOrDefault();

            tbl.ServiceID = 1;
            tbl.TyreID = x.TyreID;
            tbl.Price = x.SellingPrice;
            db.StockTbls.Add(tbl);
            db.SaveChanges();
            return RedirectToAction("ViewTyres");
        }

        public ActionResult ViewTyres()
        {

            return View(_unitOfWork.GetRepositoryInstance<TyresTbl>().GetAllRecordsIQueryable());
        }

        public ActionResult TyreEdit(int id)
        {
            return View(_unitOfWork.GetRepositoryInstance<TyresTbl>().GetFirstorDefault(id));
        }

        [HttpPost]
        public ActionResult TyreEdit(TyresTbl tbl, HttpPostedFileBase file)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var d = db.TyresTbls.FirstOrDefault(x => x.TyreID == tbl.TyreID);

            if (tbl.Image == null)
            {
                tbl.Image = new byte[file.ContentLength];
                file.InputStream.Read(tbl.Image, 0, file.ContentLength);

            }
            else
            {
                tbl.Image = d.Image;
            }
            _unitOfWork.GetRepositoryInstance<TyresTbl>().Update(tbl);

            return RedirectToAction("ViewTyres");
        }

        //Quotations Generation Admin

        public ActionResult GenerateQuotation()
        {

            return View(_unitOfWork.GetRepositoryInstance<StockTbl>().GetAllRecords().ToList());
        }

        public ActionResult ViewQuotations()
        {
            return View(_unitOfWork.GetRepositoryInstance<QuotationTbl>().GetAllRecordsIQueryable().Where(a => a.Status == "Pending" || a.Status == "Accepted"));
        }

        public ActionResult ViewQuote(int QuoteID)
        {
            ViewQuoteDetailViewModel ob = new ViewQuoteDetailViewModel();
            var customer = _unitOfWork.GetRepositoryInstance<QuotationTbl>().GetAllRecordsIQueryable().Where(a => a.QuotationID == QuoteID).ToList();

            List<QuotationDetailTbl> list = _unitOfWork.GetRepositoryInstance<QuotationDetailTbl>().GetAllRecordsIQueryable().Where(a => a.QuotationID == QuoteID).ToList();
            ob.List = list;

            foreach (var item in customer)
            {

                ob.QuoteDate = item.QuoteDate;
                ob.QuoteTotal = item.QuoteTotal;
                ob.Status = item.Status;
                ob.VAT = item.QuoteTotal * (decimal)0.15;
            }
            return View(customer);
        }
        //Start screen for booking vehicle for service/repair
        public ActionResult BookVehicle()
        {
            return View(_unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecords().ToList());
        }

        public ActionResult SelectVehicle(int customerid)
        {
            var customerveh = _unitOfWork.GetRepositoryInstance<CustomerVehicleTbl>().GetAllRecordsIQueryable().Where(a => a.CustomerID == customerid).ToList();

            return View(customerveh);
        }

        public ActionResult SelectVehicleBooking(int vehicleid)
        {
            Session["VehicleID"] = vehicleid;
            ViewBag.BayList = GetBays();
            return View();
        }

        [HttpPost]
        public ActionResult SelectVehicleBooking(BookingTbl bookingTbl)
        {
            int vehicleid = Int32.Parse(Session["VehicleID"] + "");

            bookingTbl.VehicleID = vehicleid;
            bookingTbl.Status = "Booked";

            db.BookingTbls.Add(bookingTbl);
            db.SaveChanges();

            //temp tbl storage:


            using (var dbs = new ApplicationDbContext())
            {
                ReportQuoteTempTbl ob = new ReportQuoteTempTbl();
                var booking = _unitOfWork.GetRepositoryInstance<BookingTbl>().GetAllRecords().ToList();
                var booklast = booking.Last();
                ob.BookingID = booklast.BookingID;
                dbs.ReportQuoteTempTbls.Add(ob);
                dbs.SaveChanges();
            }

            return RedirectToAction("ViewBookedVehicles", "Admin");
        }

        public ActionResult ViewBookedVehicles()
        {


            return View(_unitOfWork.GetRepositoryInstance<BookingTbl>().GetAllRecordsIQueryable().Where(a => a.HasMechanic == false));
        }

        public ActionResult AssignMechanic(int bookingid)
        {
            Session["bookid"] = bookingid;
            ViewBag.MechanicList = GetMechanics();


            return View();
        }

        [HttpPost]
        public ActionResult AssignMechanic(ScheduleTbl schedule)
        {
            int bookid = Int32.Parse(Session["bookid"].ToString());
            using (var dbs = new ApplicationDbContext())
            {
                var booking = dbs.BookingTbls.Where(a => a.BookingID == bookid).ToList();
                foreach (var item in booking)
                {
                    item.HasMechanic = true;
                    dbs.SaveChanges();
                }

            };
            schedule.Status = "Pending";
            schedule.BookingID = bookid;
            db.ScheduleTbls.Add(schedule);
            db.SaveChanges();
            return RedirectToAction("ViewBookedVehicles", "Admin");
        }

        public ActionResult CurrentMechanicBookings()
        {
            return View(_unitOfWork.GetRepositoryInstance<ScheduleTbl>().GetAllRecordsIQueryable().Where(a => a.Status == "CheckIn"));
        }


        public ActionResult AcceptQuote(int QuoteID)
        {
            using (var dbs = new ApplicationDbContext())
            {
                var quotations = dbs.QuotationTbls.Where(a => a.QuotationID == QuoteID);
                foreach (var item in quotations)
                {
                    item.Status = "Accepted";

                }
                dbs.SaveChanges();
            }

            return RedirectToAction("ViewQuotations", "Admin");
        }

        public ActionResult DeclineQuote(int QuoteID)
        {
            using (var dbs = new ApplicationDbContext())
            {
                var quotations = dbs.QuotationTbls.Where(a => a.QuotationID == QuoteID);
                foreach (var item in quotations)
                {
                    item.Status = "Declined";

                }
                dbs.SaveChanges();
            }
            return RedirectToAction("ViewQuotations", "Admin");
        }

        public ActionResult ViewMechanicReports()
        {
            return View(_unitOfWork.GetRepositoryInstance<ReportTbl>().GetAllRecordsIQueryable().Where(a => a.Status == "Pending"));
        }

        public ActionResult ReportToInvoice(int id)
        {
            Session["invid"] = id;
            return View(_unitOfWork.GetRepositoryInstance<ReportDetailTbl>().GetAllRecordsIQueryable().Where(a => a.ReportID == id));
        }
        //Payment Check for Invoice (Either Cash or Card)

        public ActionResult PaymentMethod()
        {
            int reportid = Int32.Parse(Session["invid"] + "");

            return View(_unitOfWork.GetRepositoryInstance<ReportDetailTbl>().GetAllRecordsIQueryable().Where(a => a.ReportID == reportid));
        }

        public ActionResult CashPayment(int id)
        {
            decimal total = 0;

            using (var dbs = new ApplicationDbContext())
            {
                InvoiceTbl ob = new InvoiceTbl();
                var reports = dbs.ReportTbls.Where(a => a.ReportID == id).ToList();
                foreach (var item in reports)
                {
                    item.Status = "Processed";
                    dbs.SaveChanges();
                    var cust = dbs.CustomerVehicleTbls.Where(a => a.VehicleID == item.VehicleID).ToList();
                    foreach (var customer in cust)
                    {
                        ob.VehicleID = item.VehicleID;
                        ob.CustomerID = customer.CustomerID;

                    }
                }
                // Deduct Stock In terms of Tyres if Tyres are in the Report..
                using (var stock = new ApplicationDbContext())
                {
                    var cart = stock.ReportDetailTbls.Where(a => a.ReportID == id).ToList();
                    foreach (var item in cart)
                    {
                        if (item.StockTbl.ServiceID == 1)
                        {
                            item.StockTbl.Quantity -= item.Quantity;

                        }
                        stock.SaveChanges();


                    }
                }


                var tots = dbs.ReportDetailTbls.Where(a => a.ReportID == id);
                foreach (var item in tots)
                {
                    total += (decimal)(item.Price * item.Quantity);
                }

                //Status change Back
                using (var x = new ApplicationDbContext())
                {
                    int bookid = 0;
                    int bayid = 0;
                    int mechid = 0;
                    var avail = x.ReportQuoteTempTbls.Where(a => a.ReportID == id).ToList();

                    foreach (var item in avail)
                    {
                        bookid = item.BookingID;

                    }
                    var books = x.BookingTbls.Where(a => a.BookingID == bookid).ToList();

                    foreach (var a in books)
                    {
                        bayid = a.BayID;

                    }
                    //Checkout vehicle from schedule table
                    var checkout = x.ScheduleTbls.Where(a => a.BookingID == bookid).ToList();
                    foreach (var o in checkout)
                    {
                        o.CheckedOut = true;
                        o.DateCheckOut = DateTime.Now;
                        o.Status = "Complete";
                        x.SaveChanges();
                    }

                    var bays = x.BayTbls.Where(a => a.BayID == bayid).ToList();

                    foreach (var item in bays)
                    {
                        item.IsAvailable = true;
                        x.SaveChanges();
                    }

                    var schedule = x.ScheduleTbls.Where(a => a.BookingID == bookid).ToList();
                    foreach (var item in schedule)
                    {
                        mechid = item.MechanicID;
                    }

                    var mechanic = x.MechanicTbls.Where(a => a.MechanicID == mechid).ToList();
                    foreach (var mech in mechanic)
                    {
                        mech.IsAvailable = true;
                        x.SaveChanges();
                    }

                }




                ob.FromReport = true;
                ob.FromQuotation = false;

                ob.ReportID = id;



                ob.AmountPaid = total;
                ob.PaymentType = "Cash";
                ob.DateOfInvoice = DateTime.Now;


                dbs.InvoiceTbls.Add(ob);
                dbs.SaveChanges();
            }
            return RedirectToAction("SuccessCashPayment", "Admin");

        }

        public ActionResult SuccessCashPayment()
        {
            return View();
        }

        public ActionResult ViewInvoices()
        {
            return View(_unitOfWork.GetRepositoryInstance<InvoiceTbl>().GetAllRecords());
        }

        public ActionResult InvoiceDetailViewFromReport(int id)
        {
            return View(_unitOfWork.GetRepositoryInstance<InvoiceTbl>().GetAllRecordsIQueryable().Where(a => a.InvoiceID == id));
        }

        public ActionResult BookVehicleFromQuote(int QuoteID)
        {

            return View(_unitOfWork.GetRepositoryInstance<QuotationDetailTbl>().GetAllRecordsIQueryable().Where(a => a.QuotationID == QuoteID));
        }

        public ActionResult QuoteBookVehicle(int id)
        {
            int custid = 0;
            using (var dbs = new ApplicationDbContext())
            {
                var customer = dbs.QuotationTbls.Where(a => a.QuotationID == id);

                foreach (var item in customer)
                {
                    custid = item.CustomerID;
                    Session["custid"] = item.CustomerID;
                    Session["quoteid"] = item.QuotationID;
                }
            }



            return View(_unitOfWork.GetRepositoryInstance<CustomerVehicleTbl>().GetAllRecordsIQueryable().Where(a => a.CustomerID == custid));
        }

        public ActionResult SelectVehicleQuoteBooking(int vehicleid)
        {
            Session["VehicleID"] = vehicleid;
            ViewBag.BayList = GetBays();
            return View();
        }
        [HttpPost]
        public ActionResult SelectVehicleQuoteBooking(BookingTbl booking)
        {
            int customerid = Int32.Parse(Session["custid"] + "");
            int vehicleid = Int32.Parse(Session["VehicleID"] + "");
            int quoteid = Int32.Parse(Session["quoteid"] + "");
            using (var dbs = new ApplicationDbContext())
            {
                var quotations = dbs.QuotationTbls.Where(a => a.QuotationID == quoteid);
                foreach (var item in quotations)
                {
                    item.Status = "Booked";
                    quoteid = item.QuotationID;
                }
                dbs.SaveChanges();
            }

            booking.VehicleID = vehicleid;
            booking.Status = "Booked#" + quoteid;

            db.BookingTbls.Add(booking);
            db.SaveChanges();

            using (var dbs = new ApplicationDbContext())
            {
                ReportQuoteTempTbl ob = new ReportQuoteTempTbl();
                var bookings = _unitOfWork.GetRepositoryInstance<BookingTbl>().GetAllRecords().ToList();
                var booklast = bookings.Last();
                ob.BookingID = booklast.BookingID;
                ob.QuotationID = quoteid;
                dbs.ReportQuoteTempTbls.Add(ob);
                dbs.SaveChanges();
            }
            return RedirectToAction("ViewBookedVehicles", "Admin");

        }

        public ActionResult InvoiceDetailView(int id)
        {
            return View(_unitOfWork.GetRepositoryInstance<InvoiceTbl>().GetAllRecordsIQueryable().Where(a => a.ReportID == id));
        }

        public ActionResult ViewAllOrders()
        {
            return View(_unitOfWork.GetRepositoryInstance<OrdersTbl>().GetAllRecords());
        }

        public ActionResult ViewOrderDetail(int id)
        {
            return View(_unitOfWork.GetRepositoryInstance<OrdersTbl>().GetAllRecordsIQueryable().Where(a => a.OrderID == id));
        }

        public ActionResult ViewPendingDeliveryPickUps()
        {
            return View(_unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.IsPickedUp == false));
        }

        public ActionResult NotifyDriverPickUp(int id)
        {
            var fromEmail = new MailAddress("veestyreandalignment@gmail.com", "Vees Tyre and Alignment");

            var fromEmailPassword = "ikgtqxhwvonejuat";
            var x = db.Drivers.FirstOrDefault(a => a.DriverID == id);
            var order = db.PayFastShippings.FirstOrDefault(a => a.DriverID == id);
            var toEmail = new MailAddress(x.UserID);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            MailMessage messages = new MailMessage(fromEmail, toEmail);
            messages.Body = " Hi this is friendly reminder that you still have a pending order to pick up from the warehouse";
            messages.Subject = "Reminder To Pick Up Order No: " + order.OrderID;
            messages.IsBodyHtml = false;

            smtp.Send(messages);

            return RedirectToAction("ViewPendingDeliveryPickups", "Admin");
        }

        public ActionResult ViewPendingDeliveries()
        {
            return View(_unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.IsPickedUp == true && a.Is_Delivered == false));
        }

        public ActionResult ViewEnRouteDeliveries()
        {
            return View(_unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.Status == "En-Route"));
        }
        public ActionResult ViewDeliveredOrders()
        {
            return View(_unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.Is_Delivered == true));
        }

        public ActionResult ViewDeliveredOrderDetail(int id)
        {
            var obs = _unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.PayFastShippingID == id).ToList();
            var getlast = obs.Last();
            int shipid = getlast.ShippingID;
            var ob = _unitOfWork.GetRepositoryInstance<ShippingTbl>().GetAllRecordsIQueryable().Where(a => a.ShippingID == shipid).ToList();

            CustomerTbl cust = new CustomerTbl();
            foreach (var item in ob)
            {
                string text = item.StreetAddress + " " + item.Suburb;
                string[] addresssplit = text.Split();
                string deliveryaddress = string.Join("+", addresssplit);
                ViewBag.DeliverAddress = item.StreetAddress;
                ViewBag.CurrentLocation = "430+Longbury+Drive";
                ViewBag.StringLocation = "https://www.google.com/maps/embed/v1/directions?key=AIzaSyDGQDyFapB5Q_YGUANNY1TOf27tqR1JQ6w&origin=430+Longbury+Dr,+Longcroft,+Phoenix,+4068&destination=" + deliveryaddress + "&avoid=tolls|highways";


            }
            return View(obs);
        }

        public ActionResult WorkshopBayGraphicStatus()
        {
            WorkshopBayViewModel ob = new WorkshopBayViewModel();
            ob.Baylist = _unitOfWork.GetRepositoryInstance<BayTbl>().GetAllRecords().ToList();
            return View(_unitOfWork.GetRepositoryInstance<BayTbl>().GetAllRecords().ToList());
        }

        public ActionResult TyreStockViewActive()
        {
            return View(_unitOfWork.GetRepositoryInstance<StockTbl>().GetAllRecords());
        }


        public ActionResult ViewCustomers()
        {
            return View(_unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecords());
        }
        //Cash Up sheet which will show how much cash should be in the till , the amount of Card/Visa Payments , Callouts etc , Will post to CashUpTable
        //End Feature...
        public ActionResult CashUpSheet()
        {
           
           
            return View();
        }
       [HttpPost]
       public ActionResult CashUpSheet(CashUpHistory cashUpHistory)
        {

           



            var dateAndTime = DateTime.Now;
            var date = dateAndTime.Date;

            DateTime dtEnd = date.AddDays(1).AddSeconds(-1);
            DateTime dtStart = date.AddDays(0).AddSeconds(1);

            //Get last record of CashUpsheet
            var cashup = _unitOfWork.GetRepositoryInstance<CashUpHistory>().GetAllRecords().ToList();
            var cashupgetlast = cashup.Last();
            if (cashupgetlast.CashUpDate <= dtEnd && cashupgetlast.CashUpDate >= dtStart)
            {
                return RedirectToAction("NotifyErrorSubmitCash", "Admin");
            }
            else { 


            CashUpViewModel ob = new CashUpViewModel();
            decimal invoicetotal = 0;
            int totalinvoicesales = 0;
            decimal quotationstotal = 0;
            int totalquotations = 0;
            decimal calloutinvoicetotal = 0;
            int totalcalloutinvoice = 0;
            decimal totalcashpayment = 0;
            decimal totalCardpayment = 0;
            var invoices = _unitOfWork.GetRepositoryInstance<InvoiceTbl>().GetAllRecordsIQueryable().Where(a => a.DateOfInvoice <= dtEnd && a.DateOfInvoice >= dtStart).ToList();
            foreach (var item in invoices)
            {
                invoicetotal += (decimal)item.AmountPaid;
                totalinvoicesales += 1;
                if (item.PaymentType == "Cash")
                {
                    totalcashpayment += (decimal)item.AmountPaid;
                }
                else
                {
                    totalCardpayment += (decimal)item.AmountPaid;
                }
            }
            ob.AmountSalesInStoreToday = invoicetotal;
            ob.TotalSalesInStore = totalinvoicesales;

            var quotations = _unitOfWork.GetRepositoryInstance<QuotationTbl>().GetAllRecordsIQueryable().Where(a => a.QuoteDate <= dtEnd && a.QuoteDate >= dtStart).ToList();
            foreach (var x in quotations)
            {
                quotationstotal += (decimal)x.QuoteTotal;
                totalquotations += 1;
            }
            ob.AmountOfQuotesForToday = totalquotations;
            var calloutinvoice = _unitOfWork.GetRepositoryInstance<InvoiceCalloutTbl>().GetAllRecordsIQueryable().Where(a => a.DateOfInvoice <= dtEnd && a.DateOfInvoice >= dtStart).ToList();
            foreach (var item in calloutinvoice)
            {
                calloutinvoicetotal += (decimal)item.AmountPaid;
                totalcalloutinvoice += 1;
                if (item.PaymentType == "Cash")
                {
                    totalcashpayment += (decimal)item.AmountPaid;
                }
                else
                {
                    totalCardpayment += (decimal)item.AmountPaid;
                }
            }
            ob.TotalSalesCallouts = totalcalloutinvoice;
            ob.AmountSalesCalloutsToday = calloutinvoicetotal;
            ob.TotalCardPayments = totalCardpayment;
            ob.TotalCashPayments = totalcashpayment;








            decimal totalonlinesales = 0;
            decimal totalinstoresales = 0;
            totalonlinesales = ob.TotalSalesOnline;
            totalinstoresales = ob.TotalSalesInStore;

            cashUpHistory.TotalCashPayments = ob.TotalCashPayments;
            cashUpHistory.TotalCardPayments = ob.TotalCardPayments;
            cashUpHistory.OnlineSales = totalonlinesales;
            cashUpHistory.InStoreSales = invoicetotal ;
            cashUpHistory.CashUpDate = DateTime.Now;
            db.CashUpHistories.Add(cashUpHistory);
            db.SaveChanges();
            


            return RedirectToAction("ViewCashUpSheets","Admin");
            }
        }

        public ActionResult NotifyErrorSubmitCash()
        {
            return View();
        }


    public ActionResult ViewCashUpSheets()
        {
            return View(_unitOfWork.GetRepositoryInstance<CashUpHistory>().GetAllRecords().ToList());
        }


        public ActionResult ViewToolBoxes()
        {
            return View(_unitOfWork.GetRepositoryInstance<ToolBoxTbl>().GetAllRecords());
        }

        
        /*public ActionResult AddToolsToBox(int id)
        {
            var tools = _unitOfWork.GetRepositoryInstance<ToolsTbl>().GetAllRecords();
            int toolcount = tools.Count();
            ToolBoxToolDetail ob = new ToolBoxToolDetail();
            using (var dbs = new ApplicationDbContext())
            {
               
                foreach (var item in tools)
                {
                    ob.ToolBoxID = id;
                    ob.ToolID = item.ToolID;
                    dbs.ToolBoxToolDetails.Add(ob);
                    dbs.SaveChanges();
                }
            }


                return RedirectToAction("ViewToolBoxes","Admin");
        }
        */

        public ActionResult EditToolsInBox(int id)
        {

            Session["Boxid1"] = id;

            return View(_unitOfWork.GetRepositoryInstance<ToolBoxToolDetail>().GetAllRecordsIQueryable().Where(a => a.ToolBoxID == id));
        }

        //Adding of tool to box

        public ActionResult AddToolToToolBox(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var toolcart = _unitOfWork.GetRepositoryInstance<ToolsTbl>().GetAllRecordsIQueryable();

            var tool = _unitOfWork.GetRepositoryInstance<ToolBoxToolDetail>().GetAllRecordsIQueryable().Where(a => a.ToolBoxID == id).ToList();
            foreach (var x in toolcart)
            {
                var check = db.ToolBoxToolDetails.FirstOrDefault(a => a.ToolID == x.ToolID && a.ToolBoxID == id);
                if (check == null)
                {
                    list.Add(new SelectListItem { Value = x.ToolID.ToString(), Text = x.ToolName });
                }
            }

            ViewBag.ToolsList = list;
            Session["Boxid1"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddToolToToolBox(ToolBoxToolDetail tool)
        {
            int toolboxid = Int32.Parse(Session["Boxid1"]+"");

            tool.ToolBoxID = toolboxid;
            db.ToolBoxToolDetails.Add(tool);
            db.SaveChanges();
            return RedirectToAction("EditToolsInBox","Admin", new { id = toolboxid });
        }

        public ActionResult RemoveFromToolBox(int id)
        {
            int toolboxid = Int32.Parse(Session["Boxid1"]+"");

            using (var dbs = new ApplicationDbContext())
            {
                var tooldetail = dbs.ToolBoxToolDetails.Where(a => a.ToolBoxID == toolboxid && a.ToolID == id);

                foreach (var item in tooldetail)
                {
                    dbs.ToolBoxToolDetails.Remove(item);
                 
                }
                dbs.SaveChanges();
            }
            return RedirectToAction("EditToolsInBox", "Admin", new { id = toolboxid });
        }

        public ActionResult ViewPendingRequestAssists()
        {
            return View(_unitOfWork.GetRepositoryInstance<RequestAssistanceTbl>().GetAllRecordsIQueryable().Where(a => a.Status == "Pending"));
        }

        public ActionResult AssignMechanicForRequest(int id)
        {
            Session["RequestId"] = id;
            ViewBag.MechanicList = GetMechanics();
            return View();
        }
        [HttpPost]
        public ActionResult AssignMechanicForRequest(CalloutTbl calloutTbl)
        {
            int requestid = Int32.Parse(Session["RequestId"]+"");

            using (var dbs = new ApplicationDbContext())
            {
                string link = "https://2021grp05.azurewebsites.net/Mechanic/SignArrivalNote/" + requestid;
                // GeneratedBarcode MyBarCode = QRCodeWriter.CreateQrCode(link, 500);

                QRCodeGenerator qr = new QRCodeGenerator();
                QRCodeData data = qr.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
                QRCode code = new QRCode(data);
                Bitmap qrCodeImage = code.GetGraphic(20);

                ImageConverter converter = new ImageConverter();
                var bytes = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));
                var booking = dbs.RequestAssistanceTbls.Where(a => a.RequestID == requestid).ToList();
                foreach (var item in booking)
                {
                    item.Status = "Assigned";
                    item.QRCodeCheckIn = bytes;
                    dbs.SaveChanges();
                }



            };
            //Making Mechanic Unavailable
            using (var dubs = new ApplicationDbContext())
            {
                var mech = dubs.MechanicTbls.Where(a => a.MechanicID == calloutTbl.MechanicID).ToList();
                foreach (var item in mech)
                {
                    item.IsAvailable = false;
                    dubs.SaveChanges();
                }
            }

                calloutTbl.RequestID = requestid;
            calloutTbl.IsEnRoute = false;
            calloutTbl.IsComplete = false;
            calloutTbl.IsArrived = false;
            db.CalloutTbls.Add(calloutTbl);
            db.SaveChanges();
            return RedirectToAction("AssignToolBoxForMechanic" , "Admin");
        }
        //Code For Toolbox Handling:
        
        public ActionResult AssignToolBoxForMechanic()
        {
            return View(_unitOfWork.GetRepositoryInstance<ToolBoxTbl>().GetAllRecordsIQueryable().Where(a => a.IsAvailable == true));
        }

        public ActionResult ToolBoxAssign(int id)
        {

            return View(_unitOfWork.GetRepositoryInstance<ToolBoxToolDetail>().GetAllRecordsIQueryable().Where(a => a.ToolBoxID == id));
        }

        public ActionResult ConfirmToolBox(int id)
        {
            Session["ConfirmToolBoxID"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmToolBox(ToolsCheckOut toolsCheckOut)
        {
            int toolboxid = Int32.Parse(Session["ConfirmToolBoxID"] + "");

            var callout = _unitOfWork.GetRepositoryInstance<CalloutTbl>().GetAllRecords().ToList();
            var calloutlast = callout.Last();

            using (var dbs = new ApplicationDbContext())
            {
                var toolid = dbs.ToolBoxTbls.Where(a => a.ToolBoxID == toolboxid).ToList();
                foreach (var item in toolid)
                {
                    item.IsAvailable = false;
                    item.Status = "In Use";
                    dbs.SaveChanges();
                }

            }
                toolsCheckOut.MechanicID = calloutlast.MechanicID;
            toolsCheckOut.CalloutID = calloutlast.CalloutID;

            toolsCheckOut.Status = "CheckedOut";
            
            toolsCheckOut.ToolBoxID = toolboxid;
            toolsCheckOut.IsCheckedOut = true;
            toolsCheckOut.IsCheckedIn = false;
            toolsCheckOut.DateTimeCheckedOut = DateTime.Now;

            db.ToolsCheckOuts.Add(toolsCheckOut);
            db.SaveChanges();

            return RedirectToAction("ViewCurrentCallouts","Admin");
        }
        public ActionResult ViewCurrentCallouts()
        {
            return View(_unitOfWork.GetRepositoryInstance<CalloutTbl>().GetAllRecordsIQueryable().Where(a => a.IsComplete == false));
        }

        public ActionResult ViewCalloutReports()
        {
            return View(_unitOfWork.GetRepositoryInstance<CalloutReport>().GetAllRecordsIQueryable().Where(a => a.Status == "Pending"));
        }

        public ActionResult InvoiceFromCalloutReport(int id)
        {
            Session["InvoiceCalloutreport"] = id;
            return View(_unitOfWork.GetRepositoryInstance<CalloutReportDetailTbl>().GetAllRecordsIQueryable().Where(a => a.CalloutReportID == id));
        }

        public ActionResult SendCalloutInvoice()
        {
            InvoiceCalloutTbl ob = new InvoiceCalloutTbl();
            int calloutreportid = Int32.Parse(Session["InvoiceCalloutreport"] + "");
            var calloutreport = db.CalloutReports.Where(a => a.CalloutReportID == calloutreportid).ToList();
            int mechid = 0;
            int vehicleid = 0;
            int customerid = 0;
            foreach (var item in calloutreport)
            {
                mechid = item.MechanicID;
                vehicleid = item.VehicleID;
            }

            var cust = db.CustomerVehicleTbls.Where(a => a.VehicleID == vehicleid).ToList();
            foreach (var item in cust)
            {
                customerid = item.CustomerID;
            }
            decimal total = 0;
            var tots = db.CalloutReportDetailTbls.Where(a => a.CalloutReportID == calloutreportid);
            foreach (var item in tots)
            {
                total += (decimal)(item.Price * item.Quantity);
            }
            ob.AmountPaid = total;
            ob.VehicleID = vehicleid;
            ob.CustomerID = customerid;
            ob.PaymentType = "Not Paid";
            ob.MechanicID = mechid;
            ob.CalloutReportID = calloutreportid;
            db.InvoiceCalloutTbls.Add(ob);
            db.SaveChanges();

            return RedirectToAction("ViewPendingCalloutInvoices" ,"Admin");
        }

        public ActionResult ViewPendingCalloutInvoices()
        {
            return View(_unitOfWork.GetRepositoryInstance<InvoiceCalloutTbl>().GetAllRecordsIQueryable().Where(a => a.PaymentType == "Not Paid"));
        }

        public ActionResult ToolsCheckOutHistory()
        {

            return View(_unitOfWork.GetRepositoryInstance<ToolsCheckOut>().GetAllRecordsIQueryable().Where(a => a.Status == "CheckedOut"));
        }

        public ActionResult CheckInToolBox(int id)
        {
            var toolcheckout = db.ToolsCheckOuts.Where(a => a.CalloutID == id).ToList();
            int toolboxid = 0;
            int calloutid = 0;
            
            foreach (var item in toolcheckout)
            {
                toolboxid = item.ToolBoxID;
            }


            ToolsCheckInCart ob = new ToolsCheckInCart();
            using (var dbs = new ApplicationDbContext())
            {
                var toolboxcontents = db.ToolBoxToolDetails.Where(a => a.ToolBoxID == toolboxid).ToList();
                foreach (var item in toolboxcontents)
                {
                    ob.ToolBoxID = item.ToolBoxID;
                    ob.ToolID = item.ToolID;
                    ob.CalloutID = id;
                   
                    dbs.ToolsCheckInCarts.Add(ob);
                    dbs.SaveChanges();

                }

            }
            var callID = _unitOfWork.GetRepositoryInstance<ToolsCheckInCart>().GetAllRecords().ToList();
            var getlast = callID.Last();

            int count = _unitOfWork.GetRepositoryInstance<ToolsCheckInCart>().GetAllRecordsIQueryable().Where(a => a.IsDamaged == false && a.IsMissing == false && a.IsPresent == false).ToList().Count();

            if (count == 0)
            {
                return RedirectToAction("ProceedToToolComplete", "Admin", new { id = getlast.CalloutID });
               

            }
            else
            {
                return View(_unitOfWork.GetRepositoryInstance<ToolsCheckInCart>().GetAllRecordsIQueryable().Where(a => a.IsDamaged == false && a.IsMissing == false && a.IsPresent == false).ToList());
            }
                
        }

        public ActionResult MarkToolAsPresent(int id)
        {
            var toolboxid = db.ToolsCheckInCarts.Where(a => a.ToolID == id).ToList();
            int toolbox = 0;
            foreach (var item in toolboxid)
            {
                toolbox = item.ToolBoxID;
            }

            using (var dbs = new ApplicationDbContext())
            {
                var tool = dbs.ToolsCheckInCarts.Where(a => a.ToolID == id).ToList();
                foreach (var item in tool)
                {
                    item.IsPresent = true;
                    dbs.SaveChanges();
                }
            }
                return RedirectToAction("CheckInToolBox","Admin", new { id = toolbox });
        }

        public ActionResult MarkToolAsMissing(int id)
        {
            var toolboxid = db.ToolsCheckInCarts.Where(a => a.ToolID == id).ToList();
            int toolbox = 0;
            foreach (var item in toolboxid)
            {
                toolbox = item.ToolBoxID;
            }

            using (var dbs = new ApplicationDbContext())
            {
                var tool = dbs.ToolsCheckInCarts.Where(a => a.ToolID == id).ToList();
                foreach (var item in tool)
                {
                    item.IsMissing = true;
                    dbs.SaveChanges();
                }
            }
            return RedirectToAction("CheckInToolBox", "Admin", new { id = toolbox });
        }

        public ActionResult MarkToolAsDamaged(int id)
        {
            var toolboxid = db.ToolsCheckInCarts.Where(a => a.ToolID == id).ToList();
            int toolbox = 0;
            foreach (var item in toolboxid)
            {
                toolbox = item.ToolBoxID;
            }

            using (var dbs = new ApplicationDbContext())
            {
                var tool = dbs.ToolsCheckInCarts.Where(a => a.ToolID == id).ToList();
                foreach (var item in tool)
                {
                    item.IsDamaged = true;
                    dbs.SaveChanges();
                }
            }
            return RedirectToAction("CheckInToolBox", "Admin", new { id = toolbox });
        }

        public ActionResult ProceedToToolComplete(int id)
        {
            decimal totalcostformissingdamaged = 0;
            int mechid = 0;
            int toolboxid = 0;
            var toolcheckincart = db.ToolsCheckInCarts.Where(a => a.CalloutID == id).ToList();

            foreach (var item in toolcheckincart)
            {
                if (item.IsDamaged == true || item.IsMissing == true)
                {
                    totalcostformissingdamaged += (decimal)item.ToolsTbl.ToolCost;
                    toolboxid = item.ToolBoxID;
                }
            }
            var callout = db.CalloutTbls.Where(a => a.CalloutID == id).ToList();
            foreach (var item in callout)
            {
                mechid = item.MechanicID;
            }

            //Remove tools from toolbox where Tools Damaged/Missing:

            using (var tool = new ApplicationDbContext())
            {
                var toolsinBox = tool.ToolBoxToolDetails.Where(a => a.ToolBoxID == toolboxid).ToList();
                var toolsreturned = tool.ToolsCheckInCarts.Where(a => a.CalloutID == id && a.IsPresent == false).ToList();
                foreach (var item in toolsinBox)
                {
                  foreach(var x in toolsreturned)
                    {
                        if (x.IsMissing == true || x.IsDamaged == true)
                        {
                            var edittool = tool.ToolBoxToolDetails.Where(a => a.ToolBoxID == toolboxid && a.ToolID == x.ToolID);
                            foreach (var a in edittool) 
                            {
                                tool.ToolBoxToolDetails.Remove(a);
                                
                            }
                           

                        }
                    }
                }

                tool.SaveChanges();
            }

            using (var dbs = new ApplicationDbContext())
            {
                ToolBoxCheckInHistory ob = new ToolBoxCheckInHistory();
                ob.CostOfDamagedMissingTools = totalcostformissingdamaged;
                ob.MechanicID = mechid;
                ob.CalloutID = id;
                ob.Status = "Pending";
                dbs.ToolBoxCheckInHistories.Add(ob);
                dbs.SaveChanges();

            }

                return RedirectToAction("SignToolReturn","Admin", new { callid = id });
        }
        public ActionResult SignToolReturn(int callid)
        {
            Session["COutId"] = callid;
            return View();
        }

        [HttpPost]
        public ActionResult SignToolReturn(ToolsCheckOut toolsCheckOut)
        {
            int toolboxid = 0;
            int calloutid = Int32.Parse(Session["COutId"] + "");
            int mechid = 0;
            using (var dbs = new ApplicationDbContext())
            {
                var toolcheckout = dbs.ToolsCheckOuts.Where(a => a.CalloutID == calloutid).ToList();
                foreach (var item in toolcheckout)
                {
                    item.SignatureCheckIn = toolsCheckOut.SignatureCheckIn;
                    item.IsCheckedIn = true;
                    toolboxid = item.ToolBoxID;
                    item.DateTimeReturned = DateTime.Now;
                    item.Status = "Complete";
                    mechid = item.MechanicID;
                    
                }
                dbs.SaveChanges();
            }

            using (var tool = new ApplicationDbContext())
            {
                var toolbox = tool.ToolBoxTbls.Where(a => a.ToolBoxID == toolboxid).ToList();
                foreach (var item in toolbox)
                {
                    item.IsAvailable = true;
                    item.Status = "Available";
                }
                tool.SaveChanges();

            }

            using (var dubs = new ApplicationDbContext())
            {
                var mechanic = dubs.MechanicTbls.Where(a => a.MechanicID == mechid).ToList();
                foreach (var item in mechanic)
                {
                    item.IsAvailable = true;

                }
                dubs.SaveChanges();

            }
                return RedirectToAction("ViewToolBoxHistory", "Admin");
        }

        public ActionResult ViewToolBoxHistory()
        {
            return View(_unitOfWork.GetRepositoryInstance<ToolsCheckOut>().GetAllRecordsIQueryable().Where(a => a.Status == "Complete"));
        }

        public ActionResult ReAssignDriver(int id)
        {
            ViewBag.DriverList = GetDrivers();
            Session["payidget"] = id;
            return View();
        }
        [HttpPost]
        public ActionResult ReAssignDriver(PayFastShipping payFast)
        {
            int payfastid = Int32.Parse(Session["payidget"] + "");
           
            using (var dbs = new ApplicationDbContext())
            {
                var shipping = dbs.PayFastShippings.Where(a => a.OrderID == payfastid).ToList();

                foreach (var item in shipping)
                {
                    item.DriverID = payFast.DriverID;
                    dbs.SaveChanges();

                    //Send Email notify to Driver 
                    var fromEmail = new MailAddress("veestyreandalignment@gmail.com", "Vees Tyre and Alignment");

                    var fromEmailPassword = "ikgtqxhwvonejuat";
                    var x = db.Drivers.FirstOrDefault(a => a.DriverID == item.DriverID);
                   
                    var toEmail = new MailAddress(x.UserID);
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                    };
                    //Email to Customer
                    MailMessage messages = new MailMessage(fromEmail, toEmail);
                    messages.Body = "Hi please note a delivery has been scheduled to you " +  "<br />" + "Please come report to the warehouse and pick up the order";
                    messages.Subject = "Scheduled Delivery";
                    messages.IsBodyHtml = false;

                    smtp.Send(messages);
                }
            }
            return RedirectToAction("ViewPendingDeliveryPickUps","Admin") ;
        }
        public ActionResult ViewPickUpCode(int id)
        {
            return View(_unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.PayFastShippingID == id).ToList());
        }

        public ActionResult ViewToolBoxCheckInHistory()
        {

            return View(_unitOfWork.GetRepositoryInstance<ToolBoxCheckInHistory>().GetAllRecords().ToList());
        }

        public ActionResult ViewTools()
        {
            return View(_unitOfWork.GetRepositoryInstance<ToolsTbl>().GetAllRecords());
        }
        public ActionResult ToolAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ToolAdd(ToolsTbl tools)
        {
            tools.IsDamaged = false;
            tools.IsMissing = false;
            db.ToolsTbls.Add(tools);
            db.SaveChanges();
            return RedirectToAction("ViewTools","Admin");
        }

        public ActionResult AddCustomerVehicle(int customerid)
        {
            Session["vehcustid"] = customerid;
            return View();
        }

        [HttpPost]
        public ActionResult AddCustomerVehicle(VehicleTbl vehicle)
        {
            int customerid = Int32.Parse(Session["vehcustid"] + "");
            ApplicationDbContext dbs = new ApplicationDbContext();
            vehicle.Is_Active = true;
            vehicle.Is_Delete = false;
            dbs.VehicleTbls.Add(vehicle);
            dbs.SaveChanges();


            var veh = _unitOfWork.GetRepositoryInstance<VehicleTbl>().GetAllRecords().ToList();
            var vehlast = veh.Last();

            CustomerVehicleTbl obj = new CustomerVehicleTbl();
            obj.CustomerID = customerid;
            obj.VehicleID = vehlast.VehicleID;

            db.CustomerVehicleTbls.Add(obj);
            db.SaveChanges();


            return RedirectToAction("ViewCustomers", "Admin");
        }

        public ActionResult CustomerAdd()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CustomerAdd(CustomerTbl customer)
        {
            var checkcust = db.CustomerTbls.Where(a => a.UserID == customer.EmailAddress).FirstOrDefault();

            if (checkcust == null)
            {
                customer.UserID = customer.EmailAddress;
                db.CustomerTbls.Add(customer);
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("NotifyCustomerExists", "Admin");
            }

            return RedirectToAction("ViewCustomers", "Admin");
        }

        public ActionResult NotifyCustomerExists()
        {
            return View();
        }

        public ActionResult RemindCustomerBook(int bookingid)
        {
            var booking = db.BookingTbls.Where(a => a.BookingID == bookingid).FirstOrDefault();
            var customerid = db.CustomerVehicleTbls.Where(a => a.VehicleID == booking.VehicleID).FirstOrDefault();



            var customerinfo = db.CustomerTbls.Where(a => a.CustomerID == customerid.CustomerID).FirstOrDefault();




            string custno = customerinfo.ContactNo.Remove(0, 1);
            long cellno = System.Int64.Parse("+27" + custno);





            const string YourAccessKey = "eYvbb2hpjoimCNFK9MK1mpaHk"; // your access key here
            MessageBird.Client client = MessageBird.Client.CreateDefault(YourAccessKey);
            long Msisdn = cellno; // your phone number here +27743802597




            MessageBird.Objects.Message message =
            client.SendMessage("Vees tyres",
            "Good Day "
            + "\n"
            + "\n"
            + "Please Note Your Booking Time has Passed , Please Contact the Shop"
            + "\n"
            + "\n"
            + "Thank you!"
            + "\n"
            + "\n"
            + "Kind Regards,"
            + "\n"
            + "Vees tyres"
            , new[] { Msisdn });





            return RedirectToAction("ViewBookedVehicles", "Admin");
        }
        public ActionResult ViewServices()
        {
            return View(_unitOfWork.GetRepositoryInstance<ServiceTbl>().GetAllRecords());
        }

        public ActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddService(ServiceTbl service)
        {
            service.IsActive = true;
            service.IsDelete = false;
            service.Image = null;
            service.CreatedDate = DateTime.Now;
            service.ModifiedDate = DateTime.Now;
            db.ServiceTbls.Add(service);
            db.SaveChanges();

            ApplicationDbContext dbs = new ApplicationDbContext();
            StockTbl ob = new StockTbl();
            var services = _unitOfWork.GetRepositoryInstance<ServiceTbl>().GetAllRecords();
            var getlast = services.Last();
            ob.ServiceID = getlast.ServiceID;
            ob.TyreID = null;
            ob.Price = getlast.Price;
            ob.Quantity = 0;
            dbs.StockTbls.Add(ob);
            dbs.SaveChanges();

            return RedirectToAction("ViewServices", "Admin");
        }

        public ActionResult ServiceEdit(int id)
        {
            Session["blobservid"] = id;
            return View(_unitOfWork.GetRepositoryInstance<ServiceTbl>().GetFirstorDefault(id));

        }
        [HttpPost]
        public ActionResult ServiceEdit(ServiceTbl service)
        {
            int id = Int32.Parse(Session["blobservid"] + "");

            var serv = db.ServiceTbls.Where(a => a.ServiceID == id).FirstOrDefault();

            service.CreatedDate = serv.CreatedDate;
            service.ModifiedDate = DateTime.Now;



            _unitOfWork.GetRepositoryInstance<ServiceTbl>().Update(service);
            return RedirectToAction("ViewServices", "Admin");
        }

        public ActionResult ViewReportDetails(int id)
        {
            List<ReportDetailTbl> list = _unitOfWork.GetRepositoryInstance<ReportDetailTbl>().GetAllRecordsIQueryable().Where(a => a.ReportID == id).ToList();
            var report = _unitOfWork.GetRepositoryInstance<ReportTbl>().GetAllRecordsIQueryable().Where(a => a.ReportID == id);

            return View(report);
        }


    }

}

