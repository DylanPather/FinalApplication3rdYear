using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class ToolsTbl
    {
        [Key]
        public int ToolID { get; set; }
       
        public string ToolName { get; set; }
        public string ToolBrand { get; set; }
        public decimal? ToolCost { get; set; }
        public int Quantity { get; set; }
        public bool IsMissing { get; set; }
        public bool IsDamaged { get; set; }

       

    }
}