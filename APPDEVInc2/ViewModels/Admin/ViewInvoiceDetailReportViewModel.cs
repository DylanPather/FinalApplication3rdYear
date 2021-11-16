using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APPDEVInc2.DataBaseModels;

namespace APPDEVInc2.ViewModels.Admin
{
    public class ViewInvoiceDetailReportViewModel
    {
        public int InvoiceID { get; set; }
        public int CustomerID { get; set; }
        public int VehicleID { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime? DateOfInvoice { get; set; }
        public string PaymentType { get; set; }
        public virtual CustomerTbl CustomerTbl { get; set; }
        public virtual VehicleTbl VehicleTbl { get; set; }

        public List<ReportDetailTbl> List { get; set; }


    }
}