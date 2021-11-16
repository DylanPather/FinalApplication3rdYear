using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class BayTbl
    {
        [Key]
        public int BayID { get; set; }
        public string BayName { get; set; }
        public bool IsAvailable { get; set; }
        public string Status { get; set; }
       // Consider Reference for mechanic at Bay
    }
}