using APPDEVInc2.Models;
using APPDEVInc2.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APPDEVInc2.DataBaseModels;
using System.Net.Mail;
using System.Net;

namespace APPDEVInc2.Controllers
{

     [Authorize(Roles = "Mechanic")]
    public class MechanicController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Mechanic
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult ViewCurrentBookedVehicles()
        {
            string username = this.User.Identity.Name;
            var mechanicid = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == username);
            int mechid = 0;
            foreach (var item in mechanicid)
            {
                mechid = item.MechanicID;
            }



            return View(_unitOfWork.GetRepositoryInstance<ScheduleTbl>().GetAllRecordsIQueryable().Where(a => a.MechanicID == mechid && a.Status == "Pending"));
        }

        public ActionResult AcceptBooking(int id)
        {
            using (var dbs = new ApplicationDbContext())
            {
                var checkin = dbs.ScheduleTbls.Where(a => a.ScheduleID == id).ToList();

                foreach (var item in checkin)
                {
                    item.Status = "Accepted";

                    dbs.SaveChanges();
                }
            };
            return RedirectToAction("ViewAcceptedBookings", "Mechanic");
        }

        public ActionResult ViewAcceptedBookings()
        {
            string username = this.User.Identity.Name;
            var mechanicid = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == username);
            int mechid = 0;
            foreach (var item in mechanicid)
            {
                mechid = item.MechanicID;
            }
            return View(_unitOfWork.GetRepositoryInstance<ScheduleTbl>().GetAllRecordsIQueryable().Where(a => a.CheckedIn == false && a.MechanicID == mechid && a.Status == "Accepted"));

        }

        public ActionResult CheckInVehicle(int id)
        {
            int bookid = 0;
            using (var dbs = new ApplicationDbContext())
            {
                var checkin = dbs.ScheduleTbls.Where(a => a.ScheduleID == id).ToList();

                foreach (var item in checkin)
                {
                    item.Status = "CheckIn";
                    item.CheckedIn = true;
                    item.DateCheckIn = DateTime.Now;
                    bookid = item.BookingID;
                    dbs.SaveChanges();
                }
            };

            //Sets status of Bays and Mechanic
            using (var x = new ApplicationDbContext())
            {
                int bayid = 0;
                var avail = x.BookingTbls.Where(a => a.BookingID == bookid).ToList();

                foreach (var item in avail)
                {
                    bayid = item.BayID;
                }
                var bays = x.BayTbls.Where(a => a.BayID == bayid).ToList();
                foreach (var a in bays)
                {
                    a.IsAvailable = false;
                    x.SaveChanges();
                }

                var mechanic = x.MechanicTbls.Where(a => a.UserID == this.User.Identity.Name).ToList();
                foreach (var mech in mechanic)
                {
                    mech.IsAvailable = false;
                    x.SaveChanges();
                }

            }
            return RedirectToAction("ViewCurrentMechanicalWork", "Mechanic");
        }

        public ActionResult ViewCurrentMechanicalWork()
        {
            string username = this.User.Identity.Name;
            var mechanicid = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == username);
            int mechid = 0;
            foreach (var item in mechanicid)
            {
                mechid = item.MechanicID;
            }
            return View(_unitOfWork.GetRepositoryInstance<ScheduleTbl>().GetAllRecordsIQueryable().Where(a => a.CheckedIn == true && a.MechanicID == mechid && a.Status == "CheckIn"));
        }

        public ActionResult GenerateReport(int id)
        {
            Session["Vechid"] = id;
            return View(_unitOfWork.GetRepositoryInstance<StockTbl>().GetAllRecords().ToList());
        }


        public ActionResult ReportCommit()
        {
            int vehicleid = Int32.Parse(Session["Vechid"] + "");
            string username = this.User.Identity.Name;
            var mechanicid = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == username);
            int mechid = 0;
            string userid = "";
            foreach (var item in mechanicid)
            {
                mechid = item.MechanicID;
                userid = item.UserID;
            }



            ReportTbl ob = new ReportTbl();
            ob.VehicleID = vehicleid;
            ob.MechanicID = mechid;
            ob.DateOfReport = DateTime.Now;
            ob.Status = "Pending";
            db.ReportTbls.Add(ob);
            db.SaveChanges();
            var reportid = _unitOfWork.GetRepositoryInstance<ReportTbl>().GetAllRecordsIQueryable().ToList();
            int rid = 0;
            foreach (var item in reportid)
            {
                rid = item.ReportID;
            }
            using (var dbs = new ApplicationDbContext())
            {

                var cart = dbs.ReportCarts.Where(a => a.VehicleID == userid).ToList();
                ReportDetailTbl obs = new ReportDetailTbl();
                foreach (var item in cart)
                {
                    obs.ReportID = rid;
                    obs.StockID = item.StockID;
                    obs.Quantity = item.Count;
                    obs.Price = item.StockTbl.Price;
                    obs.VAT = (decimal)(0.15) * item.StockTbl.Price;
                    dbs.ReportDetailTbls.Add(obs);
                    dbs.SaveChanges();
                }
            }

            using (var b = new ApplicationDbContext())
            {
                var cart = b.ReportCarts.Where(a => a.VehicleID == userid).ToList();

                foreach (var item in cart)
                {
                    b.ReportCarts.Remove(item);
                    b.SaveChanges();
                }
            }
            using (var dbs = new ApplicationDbContext())
            {
                var temptbl = dbs.ReportQuoteTempTbls.ToList();
                var repid = _unitOfWork.GetRepositoryInstance<ReportTbl>().GetAllRecords().ToList();
                var replast = repid.Last();
                foreach (var item in temptbl)
                {
                    if (item.ReportID == 0)
                    {
                        item.ReportID = replast.ReportID;
                        dbs.SaveChanges();
                    }

                }

            }

            return RedirectToAction("ViewReports", "Mechanic");
        }


        public ActionResult ViewReports()
        {
            // int vehicleid = Int32.Parse(Session["Vechid"] + "");
            string username = this.User.Identity.Name;
            var mechanicid = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == username);
            int mechid = 0;
            string userid = "";
            foreach (var item in mechanicid)
            {
                mechid = item.MechanicID;
                userid = item.UserID;
            }

            return View(_unitOfWork.GetRepositoryInstance<ReportTbl>().GetAllRecordsIQueryable().Where(a => a.MechanicID == mechid));
        }

        public ActionResult ViewReportDetails(int id)
        {
            return View(_unitOfWork.GetRepositoryInstance<ReportDetailTbl>().GetAllRecordsIQueryable().Where(a => a.ReportID == id));
        }

        public ActionResult GenerateReportFromQuote(int id)
        {
            string status = "";
            var booking = db.BookingTbls.Where(a => a.BookingID == id);

            foreach (var item in booking)
            {
                status = item.Status;
                Session["Vechid"] = item.VehicleID;
            }


            string[] qid = status.Split('#');
            int quoteid = Int32.Parse(qid[1]);
            ReportCart ob = new ReportCart();
            using (var dbs = new ApplicationDbContext())
            {
                var quotedetail = dbs.QuotationDetailTbls.Where(a => a.QuotationID == quoteid).ToList();

                foreach (var item in quotedetail)
                {
                    ob.StockID = item.StockID;
                    ob.Count = item.Quantity;
                    ob.VehicleID = this.User.Identity.Name;
                    ob.DateCreated = DateTime.Now;
                    dbs.ReportCarts.Add(ob);
                    dbs.SaveChanges();
                }


            }
            using (var ds = new ApplicationDbContext())
            {
                var bookings = ds.BookingTbls.Where(a => a.Status == status);
                foreach (var item in bookings)
                {
                    item.Status = "Assigned";

                }
                ds.SaveChanges();
            }



            return RedirectToAction("GenerateQuoteReportView", "Mechanic");
        }

        public ActionResult GenerateQuoteReportView()
        {
            return View(_unitOfWork.GetRepositoryInstance<StockTbl>().GetAllRecords());
        }

        public ActionResult ViewCompletedWork()
        {
            string username = this.User.Identity.Name;
            var mechanicid = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == username);
            int mechid = 0;
            foreach (var item in mechanicid)
            {
                mechid = item.MechanicID;
            }
            return View(_unitOfWork.GetRepositoryInstance<ScheduleTbl>().GetAllRecordsIQueryable().Where(a => a.MechanicID == mechid && a.Status == "Complete"));
        }

        public ActionResult ViewCurrentCallouts()
        {
            string username = this.User.Identity.Name;
            var mechanicid = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == username);
            int mechid = 0;
            foreach (var item in mechanicid)
            {
                mechid = item.MechanicID;
            }

            return View(_unitOfWork.GetRepositoryInstance<CalloutTbl>().GetAllRecordsIQueryable().Where(a => a.MechanicID == mechid && a.IsComplete == false));
        }
        public ActionResult NotifyEnRouteCallout(int id)
        {
            string mechanicname = "";
            int mechid = 0;
            var mech = db.MechanicTbls.Where(a => a.UserID == this.User.Identity.Name);
            foreach (var item in mech)
            {
                mechanicname = item.FirstName;
                mechid = item.MechanicID;
            }

            int requestid = 0;
            using (var dbs = new ApplicationDbContext())
            {
                var callout = dbs.CalloutTbls.Where(a => a.CalloutID == id).ToList();
                foreach (var item in callout)
                {
                    item.DateEnRoute = DateTime.Now;
                    item.IsEnRoute = true;
                    item.Status = "En-Route";
                    requestid = item.RequestID;
                    dbs.SaveChanges();
                }
            }

            using (var dubs = new ApplicationDbContext())
            {
                var mechanics = dubs.MechanicTbls.Where(a => a.MechanicID == mechid).ToList();
                foreach (var item in mechanics)
                {
                    item.IsAvailable = false;
                    dubs.SaveChanges();
                }
            }
                string userid = "";
            using (var data = new ApplicationDbContext())
            {
                var request = data.RequestAssistanceTbls.Where(a => a.RequestID == requestid).ToList();
                foreach (var item in request)
                {
                    item.Status = "En-Route";
                    userid = item.UserID;
                    data.SaveChanges();
                }

            }

            using (var ob = new ApplicationDbContext())
            {
                var customertbl = ob.CustomerTbls.Where(a => a.UserID == userid).ToList();
                foreach (var item in customertbl)
                {
                    string custno = item.ContactNo.Remove(0, 1);
                    long cellno = System.Int64.Parse("+27" + custno);



                    const string YourAccessKey = "eYvbb2hpjoimCNFK9MK1mpaHk"; // your access key here
                    MessageBird.Client client = MessageBird.Client.CreateDefault(YourAccessKey);
                    long Msisdn = cellno; // your phone number here +27743802597


                    MessageBird.Objects.Message message =
                    client.SendMessage("Vees tyres",
                    "Good Day "
                    + "\n"
                    + "\n"
                    + "A Mechanic Is En-Route To your Location"
                    + "\n"
                    + "\n"
                    + "Kind Regards,"
                    + "\n"
                    + "Vees tyres"
                    , new[] { Msisdn });
                }
            }

            return RedirectToAction("ViewCurrentCallouts", "Mechanic");
        }

        public ActionResult SignArrivalNote(int id)
        {
            using (var dbs = new ApplicationDbContext())
            {
                var callout = dbs.CalloutTbls.Where(a => a.RequestID == id).ToList();
                foreach (var item in callout)
                {
                    item.IsArrived = true;
                    item.Status = "Arrived";
                    item.DateArrived = DateTime.Now;
                    dbs.SaveChanges();
                }
            }

            return RedirectToAction("ViewCurrentCallouts", "Mechanic");
        }

        public ActionResult ReportForCalloutStart(int id)
        {
            Session["CalloutID"] = id;
            return View(_unitOfWork.GetRepositoryInstance<CalloutServices>().GetAllRecords());
        }

        public ActionResult SelectVehicleCallout()
        {
            int callid = Int32.Parse(Session["CalloutID"] + "");
            int request = 0;
            var callout = db.CalloutTbls.Where(a => a.CalloutID == callid);
            string userid = "";
            foreach (var item in callout)
            {
                request = item.RequestID;

            }

            var requestid = db.RequestAssistanceTbls.Where(a => a.RequestID == request);

            foreach (var item in requestid)
            {
                userid = item.UserID;

            }

            var customer = db.CustomerTbls.Where(a => a.UserID == userid);

            int customerid = 0;
            foreach (var item in customer)
            {
                customerid = item.CustomerID;
            }

            var custveh = db.CustomerVehicleTbls.Where(a => a.CustomerID == customerid);


            return View(_unitOfWork.GetRepositoryInstance<CustomerVehicleTbl>().GetAllRecordsIQueryable().Where(a => a.CustomerID == customerid));
        }

        public ActionResult SelectVehicleForCallout(int vehicleid)
        {
            CalloutReport ob = new CalloutReport();
            string username = this.User.Identity.Name;
            var mechanicid = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == username);
            int mechid = 0;
            string userid = "";
            foreach (var item in mechanicid)
            {
                mechid = item.MechanicID;
                userid = item.UserID;
            }
            ob.VehicleID = vehicleid;
            ob.MechanicID = mechid;
            ob.DateOfReport = DateTime.Now;
            ob.Status = "Pending";

            db.CalloutReports.Add(ob);
            db.SaveChanges();

            var reportid = _unitOfWork.GetRepositoryInstance<CalloutReport>().GetAllRecordsIQueryable().ToList();
            int rid = 0;
            foreach (var item in reportid)
            {
                rid = item.CalloutReportID;
            }
            using (var dbs = new ApplicationDbContext())
            {

                var cart = dbs.CalloutReportCarts.Where(a => a.VehicleID == userid).ToList();
                CalloutReportDetailTbl obs = new CalloutReportDetailTbl();
                foreach (var item in cart)
                {
                    obs.CalloutReportID = rid;
                    obs.CalloutServiceID = item.CalloutServiceID;
                    obs.Quantity = item.Count;
                    obs.Price = item.CalloutServices.Price;
                    obs.VAT = (decimal)(0.15) * item.CalloutServices.Price;
                    dbs.CalloutReportDetailTbls.Add(obs);
                    dbs.SaveChanges();
                }
            }

            using (var b = new ApplicationDbContext())
            {
                var cart = b.CalloutReportCarts.Where(a => a.VehicleID == userid).ToList();

                foreach (var item in cart)
                {
                    b.CalloutReportCarts.Remove(item);
                    b.SaveChanges();
                }
            }

            return RedirectToAction("ViewCalloutReports", "Mechanic");
        }


        public ActionResult ViewCalloutReports()
        {

            string username = this.User.Identity.Name;
            var mechanicid = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == username);
            int mechid = 0;
            string userid = "";
            foreach (var item in mechanicid)
            {
                mechid = item.MechanicID;
                userid = item.UserID;
            }

            return View(_unitOfWork.GetRepositoryInstance<CalloutReport>().GetAllRecordsIQueryable().Where(a => a.MechanicID == mechid));
        }

        public ActionResult ViewPendingCalloutInvoices()
        {
            string username = this.User.Identity.Name;
            var mechanicid = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == username);
            int mechid = 0;
            string userid = "";
            foreach (var item in mechanicid)
            {
                mechid = item.MechanicID;
                userid = item.UserID;
            }



            return View(_unitOfWork.GetRepositoryInstance<InvoiceCalloutTbl>().GetAllRecordsIQueryable().Where(a => a.MechanicID == mechid && a.PaymentType == "Not Paid"));
        }

        //Payment for callout payment , Redirect and show contents of the work and allow Cash/Card(Payfast)
        public ActionResult CapturePaymentForCalloutInvoice(int id)
        {
            int calloutreportid = 0;
            var invoicecallout = db.InvoiceCalloutTbls.Where(a => a.InvoiceCalloutID == id);

            foreach (var item in invoicecallout)
            {
                calloutreportid = item.CalloutReportID;
            }


            return View(_unitOfWork.GetRepositoryInstance<CalloutReportDetailTbl>().GetAllRecordsIQueryable().Where(a => a.CalloutReportID == calloutreportid));
        }

        public ActionResult CashPayment(int id)
        {
            decimal total = 0;
            string customername = "";
            string customersurname = "";
            string status = "";
            int vehid = 0;
            int mechid = 0;
            int check = 0;
            int custid = 0;
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
                        custid = customer.CustomerID;
                        customername = customer.CustomerTbl.FirstName;
                        customersurname = customer.CustomerTbl.Surname;
                        vehid = item.VehicleID;
                        mechid = item.MechanicID;

                    }
                }
               
                var calloutinv = dbs.InvoiceCalloutTbls.Where(a => a.CalloutReportID == id).ToList();
                foreach (var item in calloutinv)
                {
                    item.PaymentType = "Cash";
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

            }

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
            //Email to Customer
            MailMessage messages = new MailMessage(fromEmail, toEmail);
            messages.Body = "Hi , The Mechanic :"+ x.FirstName+" "+x.LastName+" that was on call for Customer : "+customername+" "+customersurname+ "<br />" +"Has Successfully completed the job and payment was received.";
            messages.Subject = "Request Assistance Notify Complete";
            messages.IsBodyHtml = false;

            smtp.Send(messages);

            var fromEmail1 = new MailAddress("veestyreandalignment@gmail.com", "Vees Tyre and Alignment");

            var fromEmailPassword1 = "ikgtqxhwvonejuat";
            var cust1 = db.CustomerTbls.FirstOrDefault(a => a.CustomerID == custid);
            // var order = db.PayFastShippings.FirstOrDefault(a => a.DriverID == id);
            var toEmail1 = new MailAddress(cust1.EmailAddress);
            var smtp1 = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail1.Address, fromEmailPassword1)
            };

            MailMessage messages1 = new MailMessage(fromEmail1, toEmail1); 
            messages.Body = "Hi " + customername+" "+customersurname+  " This is to confirm that the job is completed"+ "<br />" +"You can view your invoice under 'More Options' When you Login."+ " < br /> "+"Thank You Kindly";
            messages.Subject = "Confirmation of Callout Complete";
            messages.IsBodyHtml = false;

            smtp.Send(messages1);
            return RedirectToAction("SuccessCashPayment","Mechanic");
        }
        public ActionResult SuccessCashPayment()
        {
            return View();
        }

        public ActionResult ViewCalloutInvoices()
        {
            string username = this.User.Identity.Name;
            var mechanicid = _unitOfWork.GetRepositoryInstance<MechanicTbl>().GetAllRecordsIQueryable().Where(a => a.UserID == username);
            int mechid = 0;
            string userid = "";
            foreach (var item in mechanicid)
            {
                mechid = item.MechanicID;
                userid = item.UserID;
            }
            return View(_unitOfWork.GetRepositoryInstance<InvoiceCalloutTbl>().GetAllRecordsIQueryable().Where(a => a.DateOfInvoice != null && a.MechanicID == mechid));
        }
    }
}
