using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class MechanicTbl
    {
        [Key]
        public int MechanicID { get; set; }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleRegNo { get; set; }
        public byte[] VehicleImage { get; set; }
        public bool IsAvailable { get; set; }
    }
}