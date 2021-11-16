using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class TyresTbl
    {
        [Key]
        public int TyreID { get; set; }
        public string TyreSize { get; set; }
        public string TyreName { get; set; }
        public string TyreBrand { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string SLA { get; set; }
        public byte[] Image { get; set; }
        public bool IsFeatured { get; set; }
    }
}