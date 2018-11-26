using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonheur.Web.Models;

namespace Bonheur.Web.Controllers
{
    public class PurchaseItemsController : Controller
    {
        private BonheurStoreEntities db = new BonheurStoreEntities();

        // GET: PurchaseItems
        public ActionResult Index()
        {
            var purchaseItems = db.PurchaseItems.Include(p => p.Purchase).Include(p => p.PurchaseCategory);
            return View(purchaseItems.ToList());
        }

        // GET: PurchaseItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseItem purchaseItem = db.PurchaseItems.Find(id);
            if (purchaseItem == null)
            {
                return HttpNotFound();
            }
            return View(purchaseItem);
        }

        // GET: PurchaseItems/Create
        public ActionResult Create()
        {
            ViewBag.PurchaseID = new SelectList(db.Purchases, "PurchaseID", "InvoiceNumber");
            ViewBag.PurchaseCategoryID = new SelectList(db.PurchaseCategories, "PurchaseCategoryID", "Name");
            return View();
        }

        // POST: PurchaseItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PurchaseItemID,PurchaseCategoryID,UnitCount,Amount,PurchaseID")] PurchaseItem purchaseItem)
        {
            if (ModelState.IsValid)
            {
                db.PurchaseItems.Add(purchaseItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PurchaseID = new SelectList(db.Purchases, "PurchaseID", "InvoiceNumber", purchaseItem.PurchaseID);
            ViewBag.PurchaseCategoryID = new SelectList(db.PurchaseCategories, "PurchaseCategoryID", "Name", purchaseItem.PurchaseCategoryID);
            return View(purchaseItem);
        }

        // GET: PurchaseItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseItem purchaseItem = db.PurchaseItems.Find(id);
            if (purchaseItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.PurchaseID = new SelectList(db.Purchases, "PurchaseID", "InvoiceNumber", purchaseItem.PurchaseID);
            ViewBag.PurchaseCategoryID = new SelectList(db.PurchaseCategories, "PurchaseCategoryID", "Name", purchaseItem.PurchaseCategoryID);
            return View(purchaseItem);
        }

        // POST: PurchaseItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PurchaseItemID,PurchaseCategoryID,UnitCount,Amount,PurchaseID")] PurchaseItem purchaseItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PurchaseID = new SelectList(db.Purchases, "PurchaseID", "InvoiceNumber", purchaseItem.PurchaseID);
            ViewBag.PurchaseCategoryID = new SelectList(db.PurchaseCategories, "PurchaseCategoryID", "Name", purchaseItem.PurchaseCategoryID);
            return View(purchaseItem);
        }

        // GET: PurchaseItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseItem purchaseItem = db.PurchaseItems.Find(id);
            if (purchaseItem == null)
            {
                return HttpNotFound();
            }
            return View(purchaseItem);
        }

        // POST: PurchaseItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseItem purchaseItem = db.PurchaseItems.Find(id);
            db.PurchaseItems.Remove(purchaseItem);
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
