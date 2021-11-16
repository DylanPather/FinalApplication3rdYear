using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class QuotationTbl
    {
        [Key]
        public int QuotationID { get; set; }
        public int CustomerID { get; set; }
        public DateTime? QuoteDate { get; set; }
        public decimal? QuoteTotal { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Status { get; set; }
        public virtual CustomerTbl CustomerTbl { get; set; }

    }
}