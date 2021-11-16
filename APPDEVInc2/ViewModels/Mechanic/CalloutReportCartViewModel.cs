using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APPDEVInc2.DataBaseModels;
namespace APPDEVInc2.ViewModels.Mechanic
{
    public class CalloutReportCartViewModel
    {
        public List<CalloutReportCart> CartItems { get; set; }

        //[DataType(DataType.Currency)]
        public decimal CartTotal { get; set; }
    }
}