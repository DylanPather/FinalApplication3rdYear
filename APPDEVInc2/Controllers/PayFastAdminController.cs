using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayFast;
using PayFast.AspNet;
using APPDEVInc2.Models;
using APPDEVInc2.ViewModels;
using System.Configuration;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net;
using MessageBird;
using MessageBird.Objects;
using APPDEVInc2.ViewModels.Customer;
using APPDEVInc2.Repository;
using APPDEVInc2.DataBaseModels;
namespace APPDEVInc2.Controllers
{
    public class PayFastAdminController : Controller
    {
        public GenericUnitOfWork _UnitOfWork = new GenericUnitOfWork();
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly PayFastSettings payFastSettings;
        // GET: PayFast
        public PayFastAdminController()
        {
            this.payFastSettings = new PayFastSettings();
            this.payFastSettings.MerchantId = "10020467";
            this.payFastSettings.MerchantKey = "lumzg08u5jwv1";
            this.payFastSettings.PassPhrase = "M431216vees2323";

            this.payFastSettings.ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"];
            this.payFastSettings.ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"];
            this.payFastSettings.ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            this.payFastSettings.CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
            this.payFastSettings.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Notify()
        {
            return View();
        }
        public ActionResult PayNow(int id)
        {
            //Point of finding data for Payfast payment

            decimal total = 0;
            string customername = "";
            string customersurname = "";
            string status = "";
            int check = 0;
            
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
                        customername = customer.CustomerTbl.FirstName;
                        customersurname = customer.CustomerTbl.Surname;

                    }
                }


                var tots = dbs.ReportDetailTbls.Where(a => a.ReportID == id);
                foreach (var item in tots)
                {
                    total += (decimal)(item.Price * item.Quantity);
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
                var stat = _UnitOfWork.GetRepositoryInstance<ReportQuoteTempTbl>().GetAllRecordsIQueryable().Where(a => a.ReportID == id).ToList();
                foreach (var item in stat)
                {
                    if (item.QuotationID == 0)
                    {
                        check = 0;
                    }
                    if (item.QuotationID > 0)
                    {
                        check = 1;
                    }
                }

                if (check == 0)
                {
                    ob.FromReport = true;
                    ob.FromQuotation = false;
                }
                else
                {
                    ob.FromReport = false;
                    ob.FromQuotation = true;

                }

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
                    var bays = x.BayTbls.Where(a => a.BayID == bayid).ToList();

                    foreach (var item in bays)
                    {
                        item.IsAvailable = true;
                        x.SaveChanges();
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
                ob.ReportID = id;
                ob.AmountPaid = total;
                ob.PaymentType = "CARD/VISA";
                ob.DateOfInvoice = DateTime.Now;
               
               

                dbs.InvoiceTbls.Add(ob);
                dbs.SaveChanges();

            }
                var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

                // Merchant Details
                onceOffRequest.merchant_id = "10020467";
                onceOffRequest.merchant_key = "lumzg08u5jwv1";
                onceOffRequest.return_url = "https://2021grp05.azurewebsites.net/PayFastAdmin/Return";
                onceOffRequest.cancel_url = "https://2021grp05.azurewebsites.net/PayFastAdmin/Cancel";
                onceOffRequest.notify_url = "https://2021grp05.azurewebsites.net/PayFastAdmin/Notify";
            

                // Buyer Details
                onceOffRequest.email_address = "sbtu01@payfast.co.za";

                // Transaction Details
                onceOffRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
                onceOffRequest.amount = (double)total;



                onceOffRequest.name_first = customername;
                onceOffRequest.name_last = customersurname;
                onceOffRequest.item_name = "Payment For Instore Services";

                onceOffRequest.item_description = "Services Rendered";

                // Transaction Options
                onceOffRequest.email_confirmation = true;
                onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

                var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

                // update order totals

                return Redirect(redirectUrl);

            }




           public ActionResult Return()
            {

               
                return View();
            }


            public ActionResult Cancel()
            {
                return View();
            }

            [HttpPost]
            public ActionResult Notify([ModelBinder(typeof(PayFastNotifyModelBinder))] PayFastNotify payFastNotifyViewModel)
            {
                payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

                var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

                var isValid = payFastNotifyViewModel.signature == calculatedSignature;

                System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

                // The PayFast Validator is still under developement
                // Its not recommended to rely on this for production use cases
                var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, System.Net.IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

                var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

                System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

                var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

                System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

                // Currently seems that the data validation only works for successful payments
                if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
                {
                    var dataValidationResult = payfastValidator.ValidateData();

                    System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
                }

                if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
                {
                    System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
                }

                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
        }
    }

