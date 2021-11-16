using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APPDEVInc2.DataBaseModels;
namespace APPDEVInc2.ViewModels.Driver
{
    public class CollectOrderViewModel
    {
        public List<OrderDetailsTbl> List { get; set; }
        public List<OrdersTbl> OrdersTbl { get; set; }
        public CustomerTbl CustomerTbl { get; set; }
        public ShippingTbl ShippingTbl { get; set; }
       
        public List<PayFastShipping> PayFastShipping { get; set; }
    }
}