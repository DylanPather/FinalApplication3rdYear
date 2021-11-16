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
    public class PayFastController : Controller
    {
        public GenericUnitOfWork _UnitOfWork = new GenericUnitOfWork();
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly PayFastSettings payFastSettings;
        // GET: PayFast
        public PayFastController()
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
        public ActionResult PayNow()
        {
            var carts = _UnitOfWork.GetRepositoryInstance<Cart>().GetAllRecordsIQueryable().Where(a => a.CartID == this.User.Identity.Name);

            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartID == this.User.Identity.Name
                              select (int?)cartItems.Count *
                              cartItems.StockTbl.Price).Sum();

            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            // Merchant Details
            onceOffRequest.merchant_id = "10020467";
            onceOffRequest.merchant_key = "lumzg08u5jwv1";
            onceOffRequest.return_url = "https://2021grp05.azurewebsites.net/PayFast/Return";
            onceOffRequest.cancel_url = "https://2021grp05.azurewebsites.net/PayFast/Cancel";
            onceOffRequest.notify_url = "https://2021grp05.azurewebsites.net/PayFast/Notify";

            var shipid = db.HistoryTbls.ToArray().LastOrDefault();
            int shippingid = shipid.ShippingID;

            var postcode = _UnitOfWork.GetRepositoryInstance<ShippingTbl>().GetAllRecordsIQueryable().Where(a => a.ShippingID == shippingid).FirstOrDefault();
            int code = postcode.PostalCode;
            double delcost = 0;
            if (code > 0001 && code < 4068)
            {
                delcost = 150;
            }
            if (code == 4068)
            {
                delcost = 0;
            }
            if (code > 4068 && code < 8889)
            {
                delcost = 250;
            }
            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";

            // Transaction Details
            onceOffRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            onceOffRequest.amount = (double)total + delcost;
            var custinfo = _UnitOfWork.GetRepositoryInstance<CustomerTbl>().GetFirstorDefaultByParameter(a => a.EmailAddress == this.User.Identity.Name);


            onceOffRequest.name_first = custinfo.FirstName;
            onceOffRequest.name_last = custinfo.Surname;
            onceOffRequest.item_name = "Payment For Online Order and Delivery";
               
            onceOffRequest.item_description = "Tyres";

            // Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

            // update order totals

            return Redirect(redirectUrl);

        }

           
    

    public ActionResult Return()
    {
       
       /* const string YourAccessKey = "eYvbb2hpjoimCNFK9MK1mpaHk"; // your access key here
        MessageBird.Client client = MessageBird.Client.CreateDefault(YourAccessKey);
        const long Msisdn = +27834072658; // your phone number here +27743802597

        MessageBird.Objects.Message message =
        client.SendMessage("Vees tyres",
       "Products Purchased!"
        + "\n"
        + "\n"
        + "Good Day "
        + "\n"
        + "\n"
        + "Your order has been successful!"
        + "\n"
        + "\n"
        + "Order Number: " 
        + "\n"
        + "\n"
        + "Please Check Your Email for Receipt " 
        + "\n"
        + "\n"
        + "Thank you!"
        + "\n"
        + "\n"
        + "Kind Regards,"
        + "\n"
        + "Vees tyres"
        , new[] { Msisdn }); */
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