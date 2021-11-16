using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class QuotationDetailTbl
    {
        [Key]
         public int QuoteDetailID { get; set; }
         public int QuotationID { get; set; }
         public int StockID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal VAT { get; set; }
        public virtual QuotationTbl QuotationTbl { get; set; }
        public virtual StockTbl StockTbl { get; set; }
    }
}