using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class ReportQuoteTempTbl
    {
        [Key]
        public int ReportQuoteTempID { get; set; }
        public int BookingID { get; set; }
        public int QuotationID { get; set; }
        public int ReportID { get; set; }
       // public virtual BookingTbl BookingTbl { get; set; }
       // public virtual ReportTbl ReportTbl { get; set; }
       // public virtual QuotationTbl QuotationTbl { get; set; }

    }
}