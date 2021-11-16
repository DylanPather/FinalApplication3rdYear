using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class ScheduleTbl
    {
        [Key]
        public int ScheduleID { get; set; }
        public int BookingID { get; set; }
        public DateTime? DateCheckIn { get; set; }
        public DateTime? DateCheckOut { get; set; }
        public string Status { get; set; }
        public bool CheckedIn { get; set; }
        public bool CheckedOut { get; set; }
        public int MechanicID { get; set; }

        public virtual MechanicTbl MechanicTbl { get; set; }
        public virtual BookingTbl BookingTbl { get; set; }
    }
}