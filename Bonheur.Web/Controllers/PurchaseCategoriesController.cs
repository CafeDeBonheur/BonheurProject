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
    public class PurchaseCategoriesController : Controller
    {
        private BonheurStoreEntities db = new BonheurStoreEntities();

        // GET: PurchaseCategories
        public ActionResult Index()
        {
            return View(db.PurchaseCategories.ToList());
        }

        // GET: PurchaseCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseCategory purchaseCategory = db.PurchaseCategories.Find(id);
            if (purchaseCategory == null)
            {
                return HttpNotFound();
            }
            return View(purchaseCategory);
        }

        // GET: PurchaseCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PurchaseCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PurchaseCategoryID,Name")] PurchaseCategory purchaseCategory)
        {
            if (ModelState.IsValid)
            {
                db.PurchaseCategories.Add(purchaseCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(purchaseCategory);
        }

        // GET: PurchaseCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseCategory purchaseCategory = db.PurchaseCategories.Find(id);
            if (purchaseCategory == null)
            {
                return HttpNotFound();
            }
            return View(purchaseCategory);
        }

        // POST: PurchaseCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PurchaseCategoryID,Name")] PurchaseCategory purchaseCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchaseCategory);
        }

        // GET: PurchaseCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseCategory purchaseCategory = db.PurchaseCategories.Find(id);
            if (purchaseCategory == null)
            {
                return HttpNotFound();
            }
            return View(purchaseCategory);
        }

        // POST: PurchaseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseCategory purchaseCategory = db.PurchaseCategories.Find(id);
            db.PurchaseCategories.Remove(purchaseCategory);
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
