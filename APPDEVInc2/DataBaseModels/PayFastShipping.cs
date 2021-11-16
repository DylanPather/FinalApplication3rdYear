using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class PayFastShipping
    {
        [Key]
        public int PayFastShippingID { get; set; }
        public int DriverID { get; set; }
        public int ShippingID { get; set; }
       // [ForeignKey("OrderDetailsTbl")]
      //  [Column(Order = 4)]
        public int OrderID { get; set; }
        public string Status { get; set; }
        public bool Is_Delivered { get; set; }
        public DateTime? DateTimeDelivered { get; set; }
        public DateTime? DateTimeEnRoute { get; set; }
        public bool DeliveryNoteSigned { get; set; }
        public bool IsPickedUp { get; set; }
        public DateTime? DateTimePickedUp { get; set; }
        public Byte[] DispatchSignImage { get; set; }

        public virtual Drivers Drivers { get; set; }
        public virtual ShippingTbl ShippingTbl { get; set; }
        public virtual OrdersTbl OrdersTbl { get; set; }
        //public virtual OrderDetailsTbl OrderDetailsTbl { get; set; }

    }
}