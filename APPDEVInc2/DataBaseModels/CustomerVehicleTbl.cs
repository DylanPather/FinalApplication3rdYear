using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class CustomerVehicleTbl
    {
        [Key]
        public int CustomerVehicleID { get; set; }
        public int CustomerID { get; set; }
        public int VehicleID { get; set; }

        public virtual CustomerTbl CustomerTbl { get; set; }
        public virtual VehicleTbl VehicleTbl { get; set; }
    }
}