using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class Cart
    {
        [Key]
        public int CID { get; set; }
        public string CartID { get; set; }
        public int StockID { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual StockTbl StockTbl { get; set; }
    }
}