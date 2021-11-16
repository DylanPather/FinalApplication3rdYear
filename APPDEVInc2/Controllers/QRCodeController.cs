using APPDEVInc2.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APPDEVInc2.DataBaseModels;
namespace APPDEVInc2.Controllers
{
    public class QRCodeController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        // GET: QRCode
        public ActionResult QRCodeForPayFastShipping(int? id)
        {



            return View(_unitOfWork.GetRepositoryInstance<PayFastShipping>().GetAllRecords());
        }

       
    }
}