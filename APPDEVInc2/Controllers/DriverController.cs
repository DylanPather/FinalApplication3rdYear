using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APPDEVInc2.Repository;
using APPDEVInc2.DataBaseModels;
using APPDEVInc2.Models;
using APPDEVInc2.ViewModels.Driver;
using IronBarCode;
using QRCoder;
using Spire.Barcode;
using System.Drawing;
using System.IO;

namespace APPDEVInc2.Controllers
{
    [Authorize(Roles = "Driver")]
    public class DriverController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Driver
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            DriverDashboard ob = new DriverDashboard();
            string userid = this.User.Identity.Name;
            var driverid = _unitOfWork.GetRepositoryInstance<Drivers>().GetFirstorDefaultByParameter(a => a.UserID == userid);

            var pendingpickup = _unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.DriverID == driverid.DriverID && a.IsPickedUp == false).ToList();
            int pickupcount = pendingpickup.Count();
            ob.PendingPickUpCount = pickupcount;
            var pendingdeliveries = _unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.DriverID == driverid.DriverID && a.IsPickedUp == true && a.Is_Delivered == false).ToList();
            int deliveriescount = pendingdeliveries.Count();
            ob.DeliveriesCount = deliveriescount;
            var pendingenroute = _unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.DriverID == driverid.DriverID && a.IsPickedUp == true && a.Is_Delivered == false && a.Status == "En-Route").ToList();
            int pendingroute = pendingenroute.Count();
            ob.PendingEnRouteDeliveriesCount = pendingroute;
            
            return View(ob);
        }
        public ActionResult CollectOrder(int? id)
        {
            CollectOrderViewModel ob = new CollectOrderViewModel();
            string userid = this.User.Identity.Name;
            var driverid = _unitOfWork.GetRepositoryInstance<Drivers>().GetFirstorDefaultByParameter(a => a.UserID == userid);
            var payfastshipping = _unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.DriverID == driverid.DriverID).ToList();
            var list = new List<PayFastShipping>();
            foreach (var item in payfastshipping)
            {
                if (item.OrderID == id && item.DriverID == driverid.DriverID)
                {
                    list = _unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.OrderID == id).ToList();


                }

            }

            if (list.Count == 0)
            {
                return RedirectToAction("NotYourDelivery", "Driver");
            }
            else
            {
                return View(list);
            }


        }
        //Add return view that states not your delivery order
        public ActionResult NotYourDelivery()
        {
            return View();
        }


        public ActionResult PickUpCapture(int? id)
        {
            var deliverypay = _unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.PayFastShippingID == id).ToList();
            string userid = this.User.Identity.Name;
            var driverid = _unitOfWork.GetRepositoryInstance<Drivers>().GetFirstorDefaultByParameter(a => a.UserID == userid);
            var payfastshipping = _unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.DriverID == driverid.DriverID).ToList();
            var list = new List<PayFastShipping>();
            int count = 0;
            using (var dbs = new ApplicationDbContext())
            {
                var deliverypayfast = dbs.PayFastShippings.Where(a => a.PayFastShippingID == id).ToList();
                foreach (var x in deliverypayfast)
                {
                    if (x.PayFastShippingID == id && x.DriverID == driverid.DriverID)
                    {
                        count++;
                        x.IsPickedUp = true;
                        x.DateTimePickedUp = DateTime.Now;
                        x.Status = "Collected";
                        dbs.SaveChanges();
                    }
                   
                }
            };

            using (var ob = new ApplicationDbContext())
            {
                var payfast = ob.PayFastShippings.Where(a => a.PayFastShippingID == id).ToList();

                foreach (var x in payfast)
                {
                    if (x.PayFastShippingID == id && x.DriverID == driverid.DriverID)
                    {
                        var order = ob.OrdersTbls.Where(a => a.OrderID == x.OrderID).ToList();
                        foreach (var y in order)
                        {
                            count++;
                            y.Status = "PickedUp";
                            ob.SaveChanges();
                        }
                    }
                   
                }
            }

            if(count == 0)
            {
                return RedirectToAction("NotYourCollection","Driver");
            }
            else
            {
                return RedirectToAction("ViewDeliveries", "Driver");
            }

                
        }

        public ActionResult ViewPickUps()
        {
            string userid = this.User.Identity.Name;
            var driverid = _unitOfWork.GetRepositoryInstance<Drivers>().GetFirstorDefaultByParameter(a => a.UserID == userid);



            return View(_unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.IsPickedUp == false && a.DriverID == driverid.DriverID && a.Status == "Pending Dispatch"));
        }


        public ActionResult EnroutePost(int id)
        {
            int ordid = 0;
            using (var dbs = new ApplicationDbContext())
            {
                var payfast = dbs.PayFastShippings.Where(a => a.PayFastShippingID == id).ToList();

                foreach (var x in payfast)
                {
                    x.Status = "En-Route";
                    x.DateTimeEnRoute = DateTime.Now;
                    ordid = x.OrderID;

                    dbs.SaveChanges();

                    //Used to convert default CellNo in database to Long so sms can read

                    string custno = x.OrdersTbl.Customertbl.ContactNo.Remove(0, 1);
                    long cellno = System.Int64.Parse("+27" + custno);



                    const string YourAccessKey = "eYvbb2hpjoimCNFK9MK1mpaHk"; // your access key here
                    MessageBird.Client client = MessageBird.Client.CreateDefault(YourAccessKey);
                    long Msisdn = cellno; // your phone number here +27743802597


                    MessageBird.Objects.Message message =
                    client.SendMessage("Vees tyres",
                    "Good Day "
                    + "\n"
                    + "\n"
                    + "Delivery for order Number: " + x.OrderID + " is on route!"
                    + "\n"
                    + "\n"
                    + "Thank you!"
                    + "\n"
                    + "\n"
                    + "Kind Regards,"
                    + "\n"
                    + "Vees tyres"
                    , new[] { Msisdn });
                }



            }

            using (var x = new ApplicationDbContext())
            {
                //QR code generation!!! ,

                //var payfast = x.PayFastShippings.Where(a => a.PayFastShippingID == id).ToList();

                // ob.DispatchSignImage = MyBarCode.ToPngBinaryData();

                string link = "https://2021grp05.azurewebsites.net/Driver/SignDeliveryNote/" + ordid;
                // GeneratedBarcode MyBarCode = QRCodeWriter.CreateQrCode(link, 500);

                //New QR Code Method
                QRCodeGenerator qr = new QRCodeGenerator();
                QRCodeData data = qr.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
                QRCode code = new QRCode(data);
                Bitmap qrCodeImage = code.GetGraphic(20);

                ImageConverter converter = new ImageConverter();
                var bytes = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));

                var orderid = x.OrdersTbls.Where(a => a.OrderID == ordid).FirstOrDefault();


                orderid.Status = "En-Route";
                orderid.QRCodeImage = bytes;



                x.SaveChanges();
            };

            return RedirectToAction("ViewEnRouteDeliveries", "Driver");

        }

        public ActionResult ViewEnRouteDeliveries()
        {
            string userid = this.User.Identity.Name;
            var driverid = _unitOfWork.GetRepositoryInstance<Drivers>().GetFirstorDefaultByParameter(a => a.UserID == userid);

            return View(_unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.Status == "En-Route" && a.DriverID == driverid.DriverID));
        }

        public ActionResult ViewRoute(int id)
        {
            
            

            var ob = _unitOfWork.GetRepositoryInstance<ShippingTbl>().GetAllRecordsIQueryable().Where(a => a.ShippingID == id).ToList();
            CustomerTbl cust = new CustomerTbl();
            foreach (var item in ob)
            {
                string text = item.StreetAddress;
                string[] addresssplit = text.Split();
                string deliveryaddress = string.Join("+", addresssplit);
                ViewBag.DeliverAddress = item.StreetAddress;
                ViewBag.CurrentLocation = "430+Longbury+Drive";
                ViewBag.StringLocation = "https://www.google.com/maps/embed/v1/directions?key=AIzaSyDGQDyFapB5Q_YGUANNY1TOf27tqR1JQ6w&origin=430+Longbury+Dr,+Longcroft,+Phoenix,+4068&destination=" + deliveryaddress + "&avoid=tolls|highways";

            }
            return View(ob);
        }




        public ActionResult SignDeliveryNote(int id)
        {
            var orderid = _unitOfWork.GetRepositoryInstance<OrdersTbl>().GetAllRecordsIQueryable().Where(a => a.OrderID == id).ToList();
            Session["OrderID"] = id;
            using (var dbs = new ApplicationDbContext())
            {
                var ordid = dbs.OrdersTbls.Where(a => a.OrderID == id).ToList();

            };
                return View();
        }

        [HttpPost]
        public ActionResult SignDeliveryNote(OrdersTbl order)
        {
            int orderid = Int32.Parse(Session["OrderID"]+"");
            //byte[] bytes = Convert.FromBase64String(order.CustomerSignature.Split(',')[1]);
            using (var dbs = new ApplicationDbContext())
            {
                var orders = dbs.OrdersTbls.Where(a => a.OrderID == orderid).ToList();

                foreach (var item in orders)
                {
                    item.CustomerSignature = order.CustomerSignature;
                    dbs.SaveChanges();
                }
            }
            int ordid = 0;
            using (var db = new ApplicationDbContext())
            {
                var payfastorder = db.PayFastShippings.Where(a => a.OrderID == orderid).ToList();
                foreach (var item in payfastorder)
                {
                    item.DateTimeDelivered = DateTime.Now;
                    item.DeliveryNoteSigned = true;
                    item.Is_Delivered = true;
                    item.Status = "Complete";
                    ordid = item.OrderID;

                }
                db.SaveChanges();
            }



            using (var x = new ApplicationDbContext())
            {
                var orders1 = x.OrdersTbls.Where(a => a.OrderID == ordid);



                foreach (var item in orders1)
                {
                    item.Status = "Delivered";



                }
                x.SaveChanges();
            }
            return RedirectToAction("NoteSignedSuccess", "Driver");
        }

        public ActionResult NoteSignedSuccess()
        {
            return View();
        }


        public ActionResult ViewDeliveries()
        {
            string userid = this.User.Identity.Name;
            var driverid = _unitOfWork.GetRepositoryInstance<Drivers>().GetFirstorDefaultByParameter(a => a.UserID == userid);
            return View(_unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecordsIQueryable().Where(a => a.DriverID == driverid.DriverID && a.IsPickedUp == true && a.Is_Delivered == false));
        }

        public ActionResult NotYourCollection()
        {
            return View();
        }

    }

}