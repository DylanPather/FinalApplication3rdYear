using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class StockTbl
    {
        [Key]
        public int StockID { get; set; }
        public int ServiceID { get; set; }
        public int? TyreID { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public virtual TyresTbl TyresTbl { get; set; }
        public virtual ServiceTbl ServiceTbl { get; set; }

    }
}