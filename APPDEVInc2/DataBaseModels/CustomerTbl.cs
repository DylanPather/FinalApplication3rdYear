using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APPDEVInc2.DataBaseModels
{
    public class CustomerTbl
    {
        [Key]
        public int CustomerID { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string ContactNo { get; set; }
       
        public string UserID { get; set; }
        //public ICollection<CustomerTbl> Customer { get; set; }
    }
}