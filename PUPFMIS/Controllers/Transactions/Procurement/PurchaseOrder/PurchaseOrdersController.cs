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
//    [Route("ops/procurement/purchase-orders/{action}")]
//    public class PurchaseOrdersController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: PurchaseOrders
//        public ActionResult Index()
//        {
//            var pOHeader = db.POHeader.Include(p => p.FKSupplier);
//            return View(pOHeader.ToList());
//        }

//        // GET: PurchaseOrders/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PurchaseOrderHeader purchaseOrderHeader = db.POHeader.Find(id);
//            if (purchaseOrderHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(purchaseOrderHeader);
//        }

//        // GET: PurchaseOrders/Create
//        public ActionResult Create()
//        {
//            ViewBag.Supplier = new SelectList(db.Suppliers, "ID", "SupplierName");
//            return View();
//        }

//        // POST: PurchaseOrders/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,PONumber,PODate,Supplier")] PurchaseOrderHeader purchaseOrderHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.POHeader.Add(purchaseOrderHeader);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.Supplier = new SelectList(db.Suppliers, "ID", "SupplierName", purchaseOrderHeader.Supplier);
//            return View(purchaseOrderHeader);
//        }

//        // GET: PurchaseOrders/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PurchaseOrderHeader purchaseOrderHeader = db.POHeader.Find(id);
//            if (purchaseOrderHeader == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.Supplier = new SelectList(db.Suppliers, "ID", "SupplierName", purchaseOrderHeader.Supplier);
//            return View(purchaseOrderHeader);
//        }

//        // POST: PurchaseOrders/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,PONumber,PODate,Supplier")] PurchaseOrderHeader purchaseOrderHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(purchaseOrderHeader).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.Supplier = new SelectList(db.Suppliers, "ID", "SupplierName", purchaseOrderHeader.Supplier);
//            return View(purchaseOrderHeader);
//        }

//        // GET: PurchaseOrders/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PurchaseOrderHeader purchaseOrderHeader = db.POHeader.Find(id);
//            if (purchaseOrderHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(purchaseOrderHeader);
//        }

//        // POST: PurchaseOrders/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            PurchaseOrderHeader purchaseOrderHeader = db.POHeader.Find(id);
//            db.POHeader.Remove(purchaseOrderHeader);
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
