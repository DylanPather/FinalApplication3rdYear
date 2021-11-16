using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class InvoiceCalloutTbl
    {
        [Key]
        public int InvoiceCalloutID { get; set; }
        public int CustomerID { get; set; }
        public int VehicleID { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime? DateOfInvoice { get; set; }
        public string PaymentType { get; set; }
        public int CalloutReportID { get; set; }
        public int MechanicID { get; set; }
        public virtual MechanicTbl MechanicTbl { get; set; }
        public virtual CustomerTbl CustomerTbl { get; set; }
        public virtual VehicleTbl VehicleTbl { get; set; }
        public virtual CalloutReport CalloutReport { get; set; }
    }
}