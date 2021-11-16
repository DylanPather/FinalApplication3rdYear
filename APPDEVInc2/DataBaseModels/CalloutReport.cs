using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class CalloutReport
    {
        [Key]
        public int CalloutReportID { get; set; }
        public int VehicleID { get; set; }
        public int MechanicID { get; set; }
        public DateTime? DateOfReport { get; set; }
        public string Status { get; set; }

        public virtual VehicleTbl VehicleTbl { get; set; }
        public virtual MechanicTbl MechanicTbl { get; set; }
    }
}