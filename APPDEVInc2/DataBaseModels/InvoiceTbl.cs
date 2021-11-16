using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class InvoiceTbl
    {
        [Key]
        public int InvoiceID { get; set; }
        public int CustomerID { get; set; }
        public int VehicleID { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime? DateOfInvoice { get; set; }
        public bool FromReport { get; set; }
        public bool FromQuotation { get; set; }
        public string PaymentType { get; set; }

        public int ReportID { get; set; }
        public virtual VehicleTbl VehicleTbl { get; set; }
        public virtual CustomerTbl CustomerTbl { get; set; }
    }
}