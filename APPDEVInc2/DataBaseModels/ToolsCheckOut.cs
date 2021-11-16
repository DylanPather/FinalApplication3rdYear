using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class ToolsCheckOut
    {
        [Key]
        public int ToolBoxCheckOutID { get; set; }
        public int CalloutID { get; set; }
        public int MechanicID { get; set; }
        public int ToolBoxID { get; set; }
        public DateTime? DateTimeCheckedOut { get; set; }
        public DateTime? DateTimeReturned { get; set; }
        public bool IsCheckedOut { get; set; }
        public bool IsCheckedIn { get; set; }
        public string Status { get; set; }
        [UIHint("SignaturePad")]
        public byte[] SignatureCheckOut { get; set; }
        [UIHint("SignaturePad")]
        public byte[] SignatureCheckIn { get; set; }
        

        public virtual MechanicTbl MechanicTbl { get; set; }
        public virtual CalloutTbl CalloutTbl { get; set; }
        public virtual ToolBoxTbl ToolBoxTbl { get; set; }

    }
}