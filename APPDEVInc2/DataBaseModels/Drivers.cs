using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class Drivers
    {
        [Key]
        public int DriverID { get; set; }
        public string UserID { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleRegNo { get; set; }
        public byte[] VehicleImage { get; set; }
        public bool IsAvailable { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
    }
}