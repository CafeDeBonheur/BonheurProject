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
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
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



        public CashOpeningClosing CashOpeningClosing { get; set; }
        public TodaySalesObject TodaySalesObject { get; set; }
    }

    public class DailSalesReportObject
    {
        public DateTime? CreatedDate { get; set; }
        public Decimal Amount { get; set; }
        public int ItemSold { get; set; }
    }


    public class DailSalesDetailedReportObject
    {
        public String Name { get; set; }
        public Decimal Amount { get; set; }
        public int ItemSold { get; set; }
    }
    public class PurchaseObject
    {
        public Purchase Purchase { get; set; }
        public PurchaseItem PurchaseItem { get; set; }
        public PurchaseCategory PurchaseCategory { get; set; }
    }

    public class TodaySalesObject
    {
        public Decimal CashOpening { get; set; }

        public Decimal PettyCash { get; set; }

        public Decimal CashReturn { get; set; }
        public Decimal RunningSales { get; set; }
        public Decimal CashClosing { get; set; }
    }

    public class DashBoardObject
        {
         public Decimal TotalSale { get; set; }
        public Decimal Cashout { get; set; }
        public Decimal CashIn { get; set; }
        public Decimal CashOpening { get; set; }
        public Decimal Expenses { get; set; }

        public Decimal CashClosing { get; set; }
    }


}

