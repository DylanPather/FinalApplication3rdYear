using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class VehicleTbl
    {
        [Key]
        public int VehicleID { get; set; }
        public string VehicleRegNo { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public int VehicleMileage { get; set; }
        public byte[] VehicleImage { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Delete { get; set; }

    }
}