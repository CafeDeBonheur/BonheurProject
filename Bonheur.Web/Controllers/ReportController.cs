using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bonheur.Web.Models;
using Bonheur.Web.Models.BonheurObjects;
using System.Data.Entity.Core.Objects;

namespace Bonheur.Web.Controllers
{
    public class ReportController : Controller
    {
        private BonheurStoreEntities db = new BonheurStoreEntities();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Today()
        {

            return View();
        }

        public ActionResult DailySales()
        {
            IEnumerable<DailSalesReportObject> objs;
            var orderItems = db.OrderItems.Where(a => a.IsServed == true && a.Order.isPaid == true);
            objs = orderItems.GroupBy(a => EntityFunctions.TruncateTime(a.Order.CreatedDate)).Select(group => new DailSalesReportObject { CreatedDate  = group.Key, Amount = group.Sum(a=>a.Amount), ItemSold=group.Count() });
            return View(objs);
        }
        public ActionResult DailySalesDetails(String d)
        {
           
            IEnumerable<Order> orders;
            IEnumerable<DailSalesDetailedReportObject> objs;

            DateTime dateTo;
            OrderObject OrderObject = new OrderObject();


            if (DateTime.TryParse(d, out DateTime dateFrom))
            {
                dateTo = dateFrom.AddDays(1);
                orders = db.Orders.Where(a => a.CreatedDate < dateTo && a.CreatedDate >= dateFrom);



            }
            else
            {
                dateFrom = DateTime.Now.Date;
                dateTo = dateFrom.AddDays(1);
                orders = db.Orders.Where(a => a.CreatedDate < dateTo && a.CreatedDate >= dateFrom);
            }
            // OrderObject.OrderItems = db.OrderItems.Where(a => a.Order.CreatedDate < dateTo && a.Order.CreatedDate >= dateFrom);
            objs = orders.SelectMany(a => a.OrderItems).GroupBy(a => a.Product.Name).Select(group => new DailSalesDetailedReportObject { Name = group.Key, Amount = group.Sum(a => a.Amount), ItemSold = group.Count() });
        
           // OrderObject.Orders = orders;
            return View(objs);
        }

      
    }
}