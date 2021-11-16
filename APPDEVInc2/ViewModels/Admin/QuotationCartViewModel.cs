using APPDEVInc2.DataBaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace APPDEVInc2.ViewModels.Admin
{
    public class QuotationCartViewModel
    {
        public List<QuoteCart> CartItems { get; set; }

        //[DataType(DataType.Currency)]
        public decimal CartTotal { get; set; }
    }
}