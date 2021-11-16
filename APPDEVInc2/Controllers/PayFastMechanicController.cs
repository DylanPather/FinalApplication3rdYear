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
using System.Net.Mail;

namespace APPDEVInc2.Controllers
{
    public class PayFastMechanicController : Controller
    {
        public GenericUnitOfWork _UnitOfWork = new GenericUnitOfWork();
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly PayFastSettings payFastSettings;
        // GET: PayFast
        public PayFastMechanicController()
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
            int vehid = 0;
            int mechid = 0;
            int check = 0;
            int reqid = 0;
            using (var dbs = new ApplicationDbContext())
            {
                
                var reports = dbs.CalloutReports.Where(a => a.CalloutReportID == id).ToList();
                foreach (var item in reports)
                {

                    item.Status = "Processed";
                    dbs.SaveChanges();
                    var cust = dbs.CustomerVehicleTbls.Where(a => a.VehicleID == item.VehicleID).ToList();
                    foreach (var customer in cust)
                    {
                       
                        customername = customer.CustomerTbl.FirstName;
                        customersurname = customer.CustomerTbl.Surname;
                        vehid = item.VehicleID;
                        mechid = item.MechanicID;

                    }
                }

                var calloutinv = dbs.InvoiceCalloutTbls.Where(a => a.CalloutReportID == id).ToList();
                foreach (var item in calloutinv)
                {
                    item.PaymentType = "CARD/VISA";
                    item.DateOfInvoice = DateTime.Now;
                    dbs.SaveChanges();

                }

                var mechcallout = dbs.CalloutTbls.Where(a => a.MechanicID == mechid && a.IsArrived == true && a.IsEnRoute == true && a.IsComplete == false).ToList();

                foreach (var item in mechcallout)
                {
                    item.IsComplete = true;
                    reqid = item.RequestID;
                    item.DateComplete = DateTime.Now;
                    item.Status = "Complete";
                    dbs.SaveChanges();
                }

                var reqmech = dbs.RequestAssistanceTbls.Where(a => a.RequestID == reqid).ToList();
                foreach (var item in reqmech)
                {
                    item.Status = "Complete";
                    dbs.SaveChanges();
                }



                var callrep = dbs.CalloutReportDetailTbls.Where(a => a.CalloutReportID == id);


                var tots = dbs.CalloutReportDetailTbls.Where(a => a.CalloutReportID == id);
                foreach (var item in tots)
                {
                    total += (decimal)(item.Price * item.Quantity);
                }
              
              
               

               
               

            }
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            onceOffRequest.merchant_id = "10020467";
            onceOffRequest.merchant_key = "lumzg08u5jwv1";
            onceOffRequest.return_url = "https://2021grp05.azurewebsites.net/PayFastMechanic/Return";
            onceOffRequest.cancel_url = "https://2021grp05.azurewebsites.net/PayFastMechanic/Cancel";
            onceOffRequest.notify_url = "https://2021grp05.azurewebsites.net/PayFastMechanic/Notify";


            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            onceOffRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            onceOffRequest.amount = (double)total;



            onceOffRequest.name_first = customername;
            onceOffRequest.name_last = customersurname;
            onceOffRequest.item_name = "Payment For Callout  Services";

            onceOffRequest.item_description = "Services Rendered";

            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

            var fromEmail = new MailAddress("veestyreandalignment@gmail.com", "Vees Tyre and Alignment");

            var fromEmailPassword = "ikgtqxhwvonejuat";
            var x = db.MechanicTbls.FirstOrDefault(a => a.MechanicID == mechid);
            // var order = db.PayFastShippings.FirstOrDefault(a => a.DriverID == id);
            var toEmail = new MailAddress("veestyreandalignment@gmail.com");
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
            messages.Body = "Hi , The Mechanic :" + x.FirstName + " " + x.LastName + " that was on call for Customer : " + customername + " " + customersurname + "<br />" + "Has Successfully completed the job and payment was received.";
            messages.Subject = "Request Assistance Notify Complete";
            messages.IsBodyHtml = false;

            smtp.Send(messages);

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