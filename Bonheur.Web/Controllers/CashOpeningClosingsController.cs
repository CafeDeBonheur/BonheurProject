using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonheur.Web.Models;
using Bonheur.Web.Models.BonheurObjects;
using Newtonsoft.Json;

namespace Bonheur.Web.Controllers
{
    public class CashOpeningClosingsController : Controller
    {
        private BonheurStoreEntities db = new BonheurStoreEntities();

        public ActionResult Dashboard()
        {
            Models.BonheurObjects.DashBoardObject dashboardObj = new Models.BonheurObjects.DashBoardObject();
            DateTime dateFrom = DateTime.Now.Date;
            DateTime dateTo = DateTime.Now.Date.AddDays(1);
            IEnumerable<CashOpeningClosing> cash = db.CashOpeningClosings.Where(a => a.CreatedDate >= dateFrom && a.CreatedDate < dateTo);
            IEnumerable<Order> orders =  db.Orders.Where(a => a.CreatedDate < dateTo && a.CreatedDate >= dateFrom);
            dashboardObj.CashOpening = cash.Where(a => a.CashType == "O").Sum(a => a.Cash);
            dashboardObj.CashIn = cash.Where(a => a.CashType == "R").Sum(a => a.Cash);
            dashboardObj.Cashout = cash.Where(a => a.CashType == "P").Sum(a => a.Cash);
            dashboardObj.TotalSale =orders.Sum(a => a.Payments.Sum(b => b.OrderAmount));
            dashboardObj.CashClosing = dashboardObj.CashOpening + dashboardObj.CashIn + dashboardObj.TotalSale - dashboardObj.Cashout;
            return View(dashboardObj);
        }
        // GET: CashOpeningClosings
        public ActionResult Index()
        {
            return View(db.CashOpeningClosings.ToList());
        }

        // GET: CashOpeningClosings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashOpeningClosing cashOpeningClosing = db.CashOpeningClosings.Find(id);
            if (cashOpeningClosing == null)
            {
                return HttpNotFound();
            }
            return View(cashOpeningClosing);
        }

        public ContentResult JSON()
        {
            double count = 50, y = 10;

            Random random = new Random(DateTime.Now.Millisecond);

            List<DataPoint> dataPoints;

            dataPoints = new List<DataPoint>();

            for (int i = 0; i < count; i++)
            {
                y = y + (random.Next(0, 20) - 10);

                dataPoints.Add(new DataPoint(i, y));
            }

            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

            return Content(JsonConvert.SerializeObject(dataPoints, _jsonSetting), "application/json");
        }

        // GET: CashOpeningClosings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CashOpeningClosings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CashOpeningID,CreatedDate,Cash,CashType")] CashOpeningClosing cashOpeningClosing)
        {
            if (ModelState.IsValid)
            {
                db.CashOpeningClosings.Add(cashOpeningClosing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cashOpeningClosing);
        }

        // GET: CashOpeningClosings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashOpeningClosing cashOpeningClosing = db.CashOpeningClosings.Find(id);
            if (cashOpeningClosing == null)
            {
                return HttpNotFound();
            }
            return View(cashOpeningClosing);
        }

        // POST: CashOpeningClosings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CashOpeningID,CreatedDate,Cash,CashType")] CashOpeningClosing cashOpeningClosing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashOpeningClosing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cashOpeningClosing);
        }

        // GET: CashOpeningClosings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashOpeningClosing cashOpeningClosing = db.CashOpeningClosings.Find(id);
            if (cashOpeningClosing == null)
            {
                return HttpNotFound();
            }
            return View(cashOpeningClosing);
        }

        // POST: CashOpeningClosings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashOpeningClosing cashOpeningClosing = db.CashOpeningClosings.Find(id);
            db.CashOpeningClosings.Remove(cashOpeningClosing);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
