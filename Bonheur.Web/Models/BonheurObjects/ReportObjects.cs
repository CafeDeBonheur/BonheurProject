using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bonheur.Web.Models.BonheurObjects
{
    public class ReportObjects
    {

    }
    public class DailSalesReportObject
    {
        public DateTime? CreatedDate { get; set; }
        public Decimal Amount { get; set; }
        public int ItemSold { get; set; }
    }
    
}