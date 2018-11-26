using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bonheur.Web.Models.BonheurObjects
{
    public class BonheurObjects
    {
    }
    public class OrderObject
    {
        public Order Order { get; set; }
        public IEnumerable<Product> Product { get; set; }
        
        public Payment Payment { get; set; }

        public IEnumerable<Category> Category { get; set; }
        public Decimal RunningAmount
        { 
           get {
                Decimal runningAmount = 0;
                try
                {
                    runningAmount = Order.OrderItems.Sum(a => a.Amount);
                }
                catch (Exception)
                {

              
                }
              
            return runningAmount; }

        }

    }
}