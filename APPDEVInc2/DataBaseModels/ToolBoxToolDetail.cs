using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class ToolBoxToolDetail
    {
        [Key]
        public int ToolBoxDetailID { get; set; }
        public int ToolBoxID { get; set; }
        public int ToolID { get; set; }

        public virtual ToolsTbl ToolsTbl { get; set; }
        public virtual ToolBoxTbl ToolBoxTbl { get; set; }

    }
}