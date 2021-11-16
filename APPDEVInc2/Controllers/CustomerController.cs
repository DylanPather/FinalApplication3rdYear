using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APPDEVInc2.Repository;
using APPDEVInc2.DataBaseModels;
using APPDEVInc2.ViewModels.Customer;
using APPDEVInc2.Models;
using System.Net;
using System.Net.Mail;

namespace APPDEVInc2.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        // GET: Customer
        public ActionResult Index(string search, int? page)
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            return View(model.CreateModel(search, 8, page));

          
        }


        public ActionResult ViewOrders()
        {

            return View(_unitOfWork.GetRepositoryInstance<OrdersTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == this.User.Identity.Name).ToList());
        }

        public ActionResult ViewDeliveryStatus(int id)
        {
            var payfastshiping = _unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.OrderID == id).ToList();
            return View(payfastshiping);
        }


        public ActionResult ViewOrderDetails(int id) 
        {
            var order = _unitOfWork.GetRepositoryInstance<OrdersTbl>().GetAllRecordsIQueryable().Where(a => a.OrderID == id).ToList();
            return View(order);
        }

        public ActionResult RequestAssistance()
        {
            return View();

        }

        [HttpPost]
        public ActionResult RequestAssistance(RequestAssistanceTbl request)
        {
            //Stops multiple Requests from the same user
            var requestscheck = _unitOfWork.GetRepositoryInstance<RequestAssistanceTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == this.User.Identity.Name && a.Status == "Pending");
            ViewBag.Count = requestscheck.Count();
            if (requestscheck.Count() > 0)
            {
                return RedirectToAction("Notify", "Customer");
            }
            else
            {
                request.UserID = this.User.Identity.Name;
                request.RequestDate = DateTime.Now;
                request.Status = "Pending";
                request.QRCodeCheckIn = null;
                db.RequestAssistanceTbls.Add(request);
                db.SaveChanges();


                var fromEmail = new MailAddress("veestyreandalignment@gmail.com", "Vees Tyre and Alignment");
                var toEmail = new MailAddress("veestyreandalignment@gmail.com");
                var fromEmailPassword = "ikgtqxhwvonejuat";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };
                var customer = _unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == this.User.Identity.Name);
                string customername = "";
                string customercontact = "";

                foreach (var x in customer)
                {
                    customername = x.FirstName + " " + x.Surname;
                    customercontact = x.ContactNo;

                }
                MailMessage message = new MailMessage(fromEmail, toEmail);
                message.Body = "Pending Request For Assistance <br /> Customer Name : " + customername+ " <br />" + "Contact No : "+customercontact ;
                message.Subject = "Request Assistance for : "+ customername;
                message.IsBodyHtml = true;
               




                smtp.Send(message);

                var customertbl = _unitOfWork.GetRepositoryInstance<CustomerTbl>().GetAllRecordsIQueryable().Where(a => a.EmailAddress == this.User.Identity.Name);
                if (customertbl.Count() == 1)
                {
                    return RedirectToAction("RequestSuccess", "Customer");
                }
                else
                {
                    //Create customer profile in CustomerTable
                    return RedirectToAction("AddCustomerDetails", "Customer");
                }
            }

            
        }

        public ActionResult AddCustomerDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCustomerDetails(CustomerTbl customer)
        {
            customer.UserID = this.User.Identity.Name;
            customer.EmailAddress = this.User.Identity.Name;
            db.CustomerTbls.Add(customer);
            db.SaveChanges();
            return RedirectToAction("AddCustomerVehicle", "Customer");
        }

        public ActionResult AddCustomerVehicle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCustomerVehicle(VehicleTbl vehicle)
        {
            vehicle.VehicleImage = null;
            vehicle.Is_Active = true;
            vehicle.Is_Delete = false;
            ApplicationDbContext dbs = new ApplicationDbContext();
            dbs.VehicleTbls.Add(vehicle);
            dbs.SaveChanges();


            CustomerVehicleTbl ob = new CustomerVehicleTbl();
            var customerid = db.CustomerTbls.Where(a => a.UserID == this.User.Identity.Name).FirstOrDefault();
            ob.CustomerID = customerid.CustomerID;

            //get last recorded vehicle
            var vehicleid = _unitOfWork.GetRepositoryInstance<VehicleTbl>().GetAllRecords().ToList();
            var getlast = vehicleid.Last();
            ob.VehicleID = getlast.VehicleID;
            db.CustomerVehicleTbls.Add(ob);
            db.SaveChanges();
            return RedirectToAction("RequestSuccess", "Customer");
        }


        public ActionResult ViewCurrentRequests()
        {
            var request = _unitOfWork.GetRepositoryInstance<RequestAssistanceTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == this.User.Identity.Name && a.Status != "Complete");
            foreach (var item in request)
            {
                string text = item.RequestAddress;
                string[] addresssplit = text.Split();
                string deliveryaddress = string.Join("+", addresssplit);
                ViewBag.DeliverAddress = item.RequestAddress;
                ViewBag.CurrentLocation = "430+Longbury+Drive";
                ViewBag.StringLocation = "https://www.google.com/maps/embed/v1/directions?key=AIzaSyDGQDyFapB5Q_YGUANNY1TOf27tqR1JQ6w&origin=430+Longbury+Dr,+Longcroft,+Phoenix,+4068&destination=" + deliveryaddress + "&avoid=tolls|highways";
            }


            return View(_unitOfWork.GetRepositoryInstance<RequestAssistanceTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == this.User.Identity.Name && a.Status != "Complete" && a.Status != "Cancelled"));
        }

        public ActionResult Notify()
        {
            return View();
        }
        public ActionResult RequestSuccess()
        {
            return View();
        }

        public ActionResult ViewRequestDetail(int id)
        {
            return View(_unitOfWork.GetRepositoryInstance<RequestAssistanceTbl>().GetAllRecordsIQueryable().Where(a => a.RequestID == id));
        }

        public ActionResult ViewInvoices()
        {
            var customerid = db.CustomerTbls.Where(a => a.UserID == this.User.Identity.Name).FirstOrDefault();
           
            if (customerid ==null)
            {
                return RedirectToAction("NoInvoicesFound", "Customer");
            }
            else
            {
                var check = _unitOfWork.GetRepositoryInstance<InvoiceTbl>().GetAllRecordsIQueryable().Where(a => a.CustomerID == customerid.CustomerID).ToList();
                return View(check);

            }
        }

        public ActionResult NoInvoicesFound()
        {
            return View();
        }

        public ActionResult CancelRequest(int id)
        {
            var request = db.RequestAssistanceTbls.Where(a => a.RequestID == id);
            foreach (var item in request)
            {
                item.Status = "Cancelled";

            }
            db.SaveChanges();
            return RedirectToAction("NotifyCancelRequest", "Customer");
        }



        public ActionResult NotifyCancelRequest()
        {
            return View();
        }



    }
}