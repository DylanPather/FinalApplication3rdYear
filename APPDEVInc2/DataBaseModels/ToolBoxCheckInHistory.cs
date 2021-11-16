using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class ToolBoxCheckInHistory
    {
        [Key]
        public int ToolCheckInHistoryID { get; set; }
        public int CalloutID { get; set; }
        public int MechanicID { get; set; }
        public decimal? CostOfDamagedMissingTools { get; set; }
        public string Status { get; set; }
        public virtual CalloutTbl CalloutTbl { get; set; }
        public virtual MechanicTbl MechanicTbl { get; set; }
    }
}