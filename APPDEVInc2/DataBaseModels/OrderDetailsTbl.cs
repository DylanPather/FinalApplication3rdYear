using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class OrderDetailsTbl 
    {
        [Key]
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int StockID { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? VAT { get; set; } 
        public virtual OrdersTbl OrdersTbl { get; set; }
        public virtual StockTbl StockTbl { get; set; }


    }
}