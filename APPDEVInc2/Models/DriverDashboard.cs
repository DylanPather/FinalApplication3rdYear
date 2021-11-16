using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APPDEVInc2.Models
{
    public class DriverDashboard
    {
        public int PendingPickUpCount { get; set; }
        public int PendingEnRouteDeliveriesCount { get; set; }
        public int DeliveriesCount { get; set; }

    }
}