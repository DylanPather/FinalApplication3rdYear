using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class CalloutTbl
    {
        [Key]
        public int CalloutID { get; set; }
        public int RequestID { get; set; }
        public int MechanicID { get; set; }
        public bool IsEnRoute { get; set; }
        public bool IsArrived { get; set; }
        public bool IsComplete { get; set; }
        public string Status { get; set; }
        public DateTime? DateEnRoute { get; set; }
        public DateTime? DateArrived { get; set; }
        public DateTime? DateComplete { get; set; }
        public virtual MechanicTbl MechanicTbl { get; set; }
        public virtual RequestAssistanceTbl RequestAssistanceTbl { get; set; }

    }
}