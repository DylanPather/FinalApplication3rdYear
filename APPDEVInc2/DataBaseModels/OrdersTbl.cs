using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class OrdersTbl
    {
        [Key]
        public int OrderID { get; set; }
        
        public string UserID { get; set; }
        public string Status { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? VAT { get; set; }
        public DateTime DateOfOrder { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Delete { get; set; }
        public decimal? DeliveryCost { get; set; }
        public byte[] QRCodeImage { get; set; }
        public int CustomerID { get; set; }
        [UIHint("SignaturePad")]
        public byte[] CustomerSignature { get; set; }
       public virtual CustomerTbl Customertbl { get; set; }
       // public OrderDetailsTbl OrderDetailsTbl { get; set; }
       
        
    }
}