using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APPDEVInc2.ViewModels.Admin
{
    public class CashUpViewModel
    {
        public decimal? AmountSalesInStoreToday { get; set; }
        public decimal? AmountSalesOnlineToday { get; set; }
        public decimal? AmountSalesCalloutsToday { get; set; }
        public int AmountOfQuotesForToday { get; set; }
        public int AmountOfAlignentsForToday { get; set; }
        public int AmountOfMechanicalWorkToday { get; set; }
        public int TotalSalesInStore { get; set; }
        public int TotalSalesOnline { get; set; }
        public int TotalSalesCallouts { get; set; }
        public decimal? TotalCashPayments { get; set; }
        public decimal? TotalCardPayments { get; set; }

    }
}