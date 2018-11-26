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

namespace Bonheur.Web.Controllers
{
    public class PurchasesController : Controller
    {
        private BonheurStoreEntities db = new BonheurStoreEntities();

        // GET: Purchases
        public ActionResult Index()
        {
            var purchases = db.Purchases.Include(p => p.Supplier);
            return View(purchases.ToList().OrderBy(a=>a.CreatedDate));
        }

        // GET: Purchases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // GET: Purchases/Create
        public ActionResult Create()
        {
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name");
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PurchaseID,CreatedDate,SupplierID,InvoiceNumber")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Purchases.Add(purchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", purchase.SupplierID);
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", purchase.SupplierID);
            ViewBag.PurchaseID = new SelectList(db.Purchases, "PurchaseID", "InvoiceNumber");
            ViewBag.PurchaseCategoryID = new SelectList(db.PurchaseCategories, "PurchaseCategoryID", "Name");
            PurchaseObject purchaseOrder = new PurchaseObject
            {
                Purchase = purchase
            };
            return View(purchaseOrder);
        }

        public ActionResult FinalizePurchase(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            purchase.IsFinalize = true;
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = purchase.PurchaseID });
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PurchaseID,CreatedDate,SupplierID,InvoiceNumber")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers.OrderBy(a=>a.Name), "SupplierID", "Name", purchase.SupplierID);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Purchase purchase = db.Purchases.Find(id);
            db.Purchases.Remove(purchase);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePurchaseItem([Bind(Include = "PurchaseItemID,PurchaseCategoryID,UnitCount,Amount,PurchaseID")] PurchaseItem purchaseItem, PurchaseObject purchaseobj, int PurchaseCategoryID)
        {
            if (ModelState.IsValid)
            {
                purchaseItem.PurchaseID = purchaseobj.Purchase.PurchaseID;
                purchaseItem.PurchaseCategoryID = PurchaseCategoryID;
                db.PurchaseItems.Add(purchaseItem);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = purchaseItem.PurchaseID });
            }

            ViewBag.PurchaseID = new SelectList(db.Purchases, "PurchaseID", "InvoiceNumber", purchaseItem.PurchaseID);
            ViewBag.PurchaseCategoryID = new SelectList(db.PurchaseCategories, "PurchaseCategoryID", "Name", purchaseItem.PurchaseCategoryID);
            return View(purchaseItem);
        }

        public ActionResult DeletePurchaseItem(int id)
        {
            PurchaseItem purchaseItem = db.PurchaseItems.Find(id);
            db.PurchaseItems.Remove(purchaseItem);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = purchaseItem.PurchaseID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "PurchaseCategoryID,Name")] PurchaseCategory purchaseCategory, PurchaseObject purchaseobj)
        {
            if (ModelState.IsValid)
            {
                db.PurchaseCategories.Add(purchaseCategory);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = purchaseobj.Purchase.PurchaseID });
            }

            return View(purchaseCategory);
        }

    }
}
