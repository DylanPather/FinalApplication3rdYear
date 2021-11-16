using APPDEVInc2.DataBaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.ViewModels.Customer
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }

        //[DataType(DataType.Currency)]
        public decimal CartTotal { get; set; }

        public decimal DeliveryCost { get; set; }
    }
}