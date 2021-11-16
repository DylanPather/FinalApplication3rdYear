using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class RequestAssistanceTbl
    {
        [Key]
        public int RequestID { get; set; }
        public string UserID { get; set; }
        public string RequestAddress { get; set; }
        public string TravelMode { get; set; }
        public string TotalTime { get; set; }
        public string TotalDistance { get; set; }
        public string DescriptionOfProblem { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Status { get; set; }
        public byte[] QRCodeCheckIn { get; set; }
    }
}