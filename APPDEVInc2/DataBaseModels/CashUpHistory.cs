using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class CashUpHistory
    {
        [Key]
        public int CashUpID { get; set; }
        public DateTime? CashUpDate { get; set; }
        public decimal? OnlineSales { get; set; }
        public decimal? InStoreSales { get; set; }
        public decimal? DailyExpense { get; set; }
        public decimal? TillFloat { get; set; }
        public decimal? TotalCashPayments { get; set; }
        public decimal? TotalCardPayments { get; set; }
    }
}