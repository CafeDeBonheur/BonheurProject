using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bonheur.Web.Models;
using Bonheur.Web.Models.BonheurObjects;
using System.Data.Entity;

namespace Bonheur.Web.Controllers
{

    public class OrdersController : Controller
    {
        private BonheurStoreEntities db = new BonheurStoreEntities();
        // GET: Orders
        //public ActionResult Index()
        //{

        //    DateTime dateNow = DateTime.Now.AddDays(-1);
        //    IEnumerable<Order> orders = db.Orders.Where(a => a.CreatedDate > dateNow || a.isPaid==false);
        //    return View(orders);
        //}

        //[ActionName("IndexSearch"]
        public ActionResult Index(String d)
        {
            IEnumerable<Order> orders;

            
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
            OrderObject.OrderItems = db.OrderItems.Where(a => a.IsServed == false);

            try
            {
                IEnumerable< CashOpeningClosing> cashopeningclosings = db.CashOpeningClosings.Where(a=>a.CreatedDate < dateTo && a.CreatedDate >= dateFrom);
                
                TodaySalesObject todaySale = new TodaySalesObject();
                todaySale.CashOpening = cashopeningclosings.Where(a => a.CashType.Trim() == "O").Sum(a => a.Cash);
                todaySale.CashReturn = cashopeningclosings.Where(a => a.CashType.Trim() == "R").Sum(a => a.Cash);
                todaySale.PettyCash = cashopeningclosings.Where(a => a.CashType.Trim() == "P").Sum(a => a.Cash);
                todaySale.RunningSales = orders.Sum(a => a.Payments.Sum(b => b.OrderAmount));//  OrderObject.OrderItems.Sum(a => a.Amount);
                todaySale.CashClosing = todaySale.CashOpening + todaySale.RunningSales - todaySale.PettyCash + todaySale.CashReturn;
                OrderObject.TodaySalesObject = todaySale;
            }
            catch 
            {

               
            }
            OrderObject.Orders = orders;
            return View(OrderObject);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id=0)
        {
            try
            {
                Order order = db.Orders.Find(id);
                OrderObject OrderObject = new OrderObject
                {
                    Order = order,
                    Product = db.Products,
                    Category = db.Categories

                };
                return View(OrderObject);
            }
            catch (Exception)
            {

                throw;

            }






        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details([Bind(Include = "OrderID,CustomerName,CreatedDate")] Order order)
        {
            if (ModelState.IsValid)
            {

                Order order1 = db.Orders.Find(order.OrderID);
                //db.Entry(order).State = EntityState.Modified;
                order1.CustomerName = order.CustomerName;
                order1.CreatedDate = order.CreatedDate;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = order.OrderID });
            }

            return View(order);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Details(int id,String Order_CustomerName, String Order_CreatedDate = "")
        //{
        //    if (ModelState.IsValid)
        //    {

        //        Order order1 = db.Orders.Find(id);
        //        //db.Entry(order).State = EntityState.Modified;
        //        order1.CustomerName = Order_CustomerName;
        //        order1.CreatedDate = DateTime.Now;
        //        db.SaveChanges();
        //        return RedirectToAction("Details", new { id = id });
        //    }

        //    return View();
        //}

        // GET: Orders/Create
        public ActionResult Create()
        {
            Order order = new Order
            {
                CustomerID = 1,
                isPaid = false,

                CustomerName = "Customer",
                CreatedDate = DateTime.Now
            };
            db.Orders.Add(order);
            db.SaveChanges();
            return RedirectToAction("Details", new{  id = order.OrderID});
           // return View();
        }

        // POST: Orders/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }



        public ActionResult AddOrder(int id = 0,int orderid=0)
        {
            Product product = db.Products.Find(id);
            OrderItem orderitem = new OrderItem
            {
                Amount = product.Amount,
                OrderID = orderid,
                ProductID = product.ProductID
            };

            db.OrderItems.Add(orderitem);
            db.SaveChanges();
            
            return RedirectToAction("Details", new { id = orderid });
        }

        public ActionResult RemoveOrder(int id = 0, int orderid = 0)
        {

            OrderItem orderitem = db.OrderItems.Find(id);
            db.OrderItems.Remove(orderitem);
           

         
            db.SaveChanges();

            return RedirectToAction("Details", new { id = orderid });
        }

        public ActionResult _AddPayment(int id=0)
        {
            Order order = db.Orders.Find(id);
            OrderObject orderobject = new OrderObject
            {
                Order = order
            };

            return PartialView(orderobject);
        }



        [HttpPost]
        public ActionResult _AddPayment(OrderObject orderObject)
        {
            try
            {
                Order order = db.Orders.Find(orderObject.Order.OrderID);
                Payment payment = new Payment
                {
                    OrderAmount = order.OrderItems.Sum(a => a.Amount) - order.Payments.Sum(a => a.Amount),
                    Amount = orderObject.Payment.Amount
                };
                payment.ChangeAmount = payment.Amount - payment.OrderAmount;
                payment.OrderID = orderObject.Order.OrderID;
                if(payment.ChangeAmount>=0)
                { 
                db.Payments.Add(payment);
                if (orderObject.RunningAmount<=0)
                {
                    
                    order.isPaid = true;
                }
                db.SaveChanges();
                // TODO: Add insert logic here

                return RedirectToAction("Details", new { id = orderObject.Order.OrderID });
                }
                else
                {
                    TempData["error"] = "Fail to add payment";
                    return RedirectToAction("Details", new { id = orderObject.Order.OrderID });
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SetServed(int id)
        {
            OrderItem orderitem = db.OrderItems.Find(id);
            orderitem.IsServed = true;


            db.SaveChanges();
            return RedirectToAction("Details", new { id = orderitem.OrderID});
           // return View();
        }

        public ActionResult SetPaid(int id)
        {
            Order order = db.Orders.Find(id);
            order.isPaid = true;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = order.OrderID });
            // return View();
        }

      
        // POST: Orders/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Orders/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeletePayment(int id)
        {
            Payment payment= db.Payments.Find(id);
            db.Payments.Remove(payment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = payment.OrderID});
        }

        public ActionResult _AddOpeningCash()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _AddOpeningCash([Bind(Include = "Cash")] CashOpeningClosing cashOpeningClosing)
        {
            if (ModelState.IsValid)
            {
                cashOpeningClosing.CashType = "O";
                cashOpeningClosing.CreatedDate = DateTime.Now;
                db.CashOpeningClosings.Add(cashOpeningClosing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cashOpeningClosing);
        }

        public ActionResult _AddPettyCash()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _AddPettyCash([Bind(Include = "Cash")] CashOpeningClosing cashOpeningClosing)
        {
            if (ModelState.IsValid)
            {
                cashOpeningClosing.CashType = "P";
                cashOpeningClosing.CreatedDate = DateTime.Now;
                db.CashOpeningClosings.Add(cashOpeningClosing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cashOpeningClosing);
        }
        public ActionResult _ReturnCash()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ReturnCash([Bind(Include = "Cash")] CashOpeningClosing cashOpeningClosing)
        {
            if (ModelState.IsValid)
            {
                cashOpeningClosing.CashType = "R";
                cashOpeningClosing.CreatedDate = DateTime.Now;
                db.CashOpeningClosings.Add(cashOpeningClosing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cashOpeningClosing);
        }


    }
}
