using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class ShippingTbl
    {
        [Key]
        public int ShippingID { get; set; }
        public string UserID { get; set; }
        public string StreetAddress { get; set; }
        public string CompBuilding { get; set; }
        public string City_Town { get; set; }
        public string Province { get; set; }
        public string Suburb { get; set; }
        public int PostalCode { get; set; }

        
    }
}