//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;

//namespace PUPFMIS.Controllers
//{
//    [Route("ops/procurement/supplier-evaluation/{action}")]
//    public class SupplierEvaluationController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: SupplierEvaluation
//        public ActionResult Index()
//        {
//            var supplierEvaluation = db.SupplierEvaluation.Include(s => s.FKItems).Include(s => s.FKOffices).Include(s => s.FKPurchaseOrder);
//            return View(supplierEvaluation.ToList());
//        }

//        // GET: SupplierEvaluation/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SupplierEvaluation supplierEvaluation = db.SupplierEvaluation.Find(id);
//            if (supplierEvaluation == null)
//            {
//                return HttpNotFound();
//            }
//            return View(supplierEvaluation);
//        }

//        [Route("ops/procurement/supplier-evaluation/{type}/create")]
//        public ActionResult Create(string type)
//        {
//            if(type == "end-user")
//            {
//                ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName");
//                ViewBag.EndUser = new SelectList(db.Offices, "ID", "OfficeCode");
//                ViewBag.PurchaseOrderID = new SelectList(db.POHeader, "ID", "PONumber");
//                return View();
//            }
//            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        }

//        // POST: SupplierEvaluation/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,PurchaseOrderID,SupplierID,EndUser,ItemID")] SupplierEvaluation supplierEvaluation)
//        {
//            if (ModelState.IsValid)
//            {
//                db.SupplierEvaluation.Add(supplierEvaluation);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName", supplierEvaluation.ItemID);
//            ViewBag.EndUser = new SelectList(db.Offices, "ID", "OfficeCode", supplierEvaluation.EndUser);
//            ViewBag.PurchaseOrderID = new SelectList(db.POHeader, "ID", "PONumber", supplierEvaluation.PurchaseOrderID);
//            return View(supplierEvaluation);
//        }

//        // GET: SupplierEvaluation/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SupplierEvaluation supplierEvaluation = db.SupplierEvaluation.Find(id);
//            if (supplierEvaluation == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName", supplierEvaluation.ItemID);
//            ViewBag.EndUser = new SelectList(db.Offices, "ID", "OfficeCode", supplierEvaluation.EndUser);
//            ViewBag.PurchaseOrderID = new SelectList(db.POHeader, "ID", "PONumber", supplierEvaluation.PurchaseOrderID);
//            return View(supplierEvaluation);
//        }

//        // POST: SupplierEvaluation/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,PurchaseOrderID,SupplierID,EndUser,ItemID")] SupplierEvaluation supplierEvaluation)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(supplierEvaluation).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName", supplierEvaluation.ItemID);
//            ViewBag.EndUser = new SelectList(db.Offices, "ID", "OfficeCode", supplierEvaluation.EndUser);
//            ViewBag.PurchaseOrderID = new SelectList(db.POHeader, "ID", "PONumber", supplierEvaluation.PurchaseOrderID);
//            return View(supplierEvaluation);
//        }

//        // GET: SupplierEvaluation/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SupplierEvaluation supplierEvaluation = db.SupplierEvaluation.Find(id);
//            if (supplierEvaluation == null)
//            {
//                return HttpNotFound();
//            }
//            return View(supplierEvaluation);
//        }

//        // POST: SupplierEvaluation/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            SupplierEvaluation supplierEvaluation = db.SupplierEvaluation.Find(id);
//            db.SupplierEvaluation.Remove(supplierEvaluation);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
