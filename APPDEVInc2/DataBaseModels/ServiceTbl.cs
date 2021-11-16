﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class ServiceTbl
    {
        [Key]
        public int ServiceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public byte[] Image { get; set; }
        public decimal Price { get; set; }
        
    }
}