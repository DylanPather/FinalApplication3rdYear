using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class ToolsCheckInCart
    {
        [Key]
        public int ToolCheckInID { get; set; }
        public int CalloutID { get; set; }
        public int ToolBoxID { get; set; }
        public int ToolID { get; set; }
        public bool IsPresent { get; set; }
        public bool IsMissing { get; set; }
        public bool IsDamaged { get; set; }

        public virtual CalloutTbl CalloutTbl { get; set; }
        public virtual ToolsTbl ToolsTbl { get; set; }
        public virtual ToolBoxTbl ToolBoxTbl { get; set; }

    }
}