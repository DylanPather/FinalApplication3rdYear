using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class HistoryTbl
    {
        [Key]
        public int HistID { get; set; }
        public string UserID { get; set; }
        public int ShippingID { get; set; }

        public int OrderID { get; set; }
    }
}