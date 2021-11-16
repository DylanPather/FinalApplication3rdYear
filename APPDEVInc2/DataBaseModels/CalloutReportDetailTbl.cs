using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class CalloutReportDetailTbl
    {
        [Key]
        public int CalloutReportDetailID { get; set; }
        public int CalloutReportID { get; set; }
        public int CalloutServiceID { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? VAT { get; set; }

        public virtual CalloutReport CalloutReport { get; set; }
        public virtual CalloutServices CalloutServices { get; set; }
    }
}