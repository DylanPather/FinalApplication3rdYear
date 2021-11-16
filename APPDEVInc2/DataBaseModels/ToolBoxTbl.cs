using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class ToolBoxTbl
    {
        [Key]
        public int ToolBoxID { get; set; }
        public string ToolBoxColor { get; set; }
        public bool IsAvailable { get; set; }
        public string Status { get; set; }
    }
}