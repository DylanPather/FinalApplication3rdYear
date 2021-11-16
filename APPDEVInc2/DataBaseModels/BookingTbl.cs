using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class BookingTbl
    {
        [Key]
        public int BookingID { get; set; }
        public int VehicleID { get; set; }
        public int BayID { get; set; }
        [Display(Name = "Date and Time Booked")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? DateBooked { get; set; }
        public string Status { get; set; }
        public bool HasMechanic { get; set; }

        public virtual VehicleTbl VehicleTbl { get; set; }
    }
}