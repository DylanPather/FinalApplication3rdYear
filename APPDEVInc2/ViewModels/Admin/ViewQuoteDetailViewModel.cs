using APPDEVInc2.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APPDEVInc2.ViewModels.Admin
{
    public class ViewQuoteDetailViewModel
    {
        public int QuotationID { get; set; }
        public int CustomerID { get; set; }
        public DateTime? QuoteDate { get; set; }
        public decimal? QuoteTotal { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Status { get; set; }
        public virtual CustomerTbl CustomerTbl { get; set; }
        public virtual StockTbl StockTbl { get; set; }
        public List<QuotationDetailTbl> List { get; set; }
        public decimal? VAT { get; set; }

    }
}