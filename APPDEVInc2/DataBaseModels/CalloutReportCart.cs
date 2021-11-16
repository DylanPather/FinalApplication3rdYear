using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class CalloutReportCart
    {
        [Key]
        public int RCID { get; set; }
        public string VehicleID { get; set; }
        public int CalloutServiceID { get; set; }
        public int Count { get; set; }
        public DateTime? DateCreated { get; set; }
        public virtual CalloutServices CalloutServices { get; set; }
    }
}