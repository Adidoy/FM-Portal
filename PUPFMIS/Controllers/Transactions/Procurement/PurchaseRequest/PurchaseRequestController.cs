//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;

//namespace PUPFMIS.Controllers.Transactions.Procurement
//{
//    [Route("ops/procurement/purchase-requests/")]
//    public class PurchaseRequestController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        [Route("ops/procurement/purchase-requests/{type}/index")]
//        public ActionResult Index(string type)
//        {
//            if(type == "procurement-office")
//            {
//                var purchaseRequest = db.PurchaseRequest.Include(p => p.FK_Department).Include(p => p.FK_Section);
//                return View("IndexProcurement", purchaseRequest.ToList());
//            }
//            else if(type == "requesting-office")
//            {
//                var purchaseRequest = db.PurchaseRequest.Include(p => p.FK_Department).Include(p => p.FK_Section);
//                return View("IndexRequesting", purchaseRequest.ToList());
//            }
//            return HttpNotFound();
//        }

//        // GET: PurchaseRequest/Details/5
//        //public ActionResult Details(int? id)
//        //{
//        //    if (id == null)
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    PurchaseRequest purchaseRequest = db.PurchaseRequest.Find(id);
//        //    if (purchaseRequest == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(purchaseRequest);
//        //}
//        [Route("ops/procurement/purchase-requests/{type}/details")]
//        public ActionResult Details(string type)
//        {
//            if (type == "procurement-office")
//            {
//                //query by office
//                return View("DetailsProcurement");
//            }
//            else if (type == "requesting-office")
//            {
//                return View("DetailsRequesting");
//            }
//            return HttpNotFound();
//        }
//        [Route("ops/procurement/purchase-requests/create")]
//        // GET: PurchaseRequest/Create
//        public ActionResult Create()
//        {
//            ViewBag.Department = new SelectList(db.Offices, "ID", "OfficeName");
//            ViewBag.Section = new SelectList(db.Offices, "ID", "OfficeName");
//            return View();
//        }

//        // POST: PurchaseRequest/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,PurchaseRequestNo,Department,Section")] PurchaseRequest purchaseRequest)
//        {
//            if (ModelState.IsValid)
//            {
//                db.PurchaseRequest.Add(purchaseRequest);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.Department = new SelectList(db.Offices, "ID", "OfficeCode", purchaseRequest.Department);
//            ViewBag.Section = new SelectList(db.Offices, "ID", "OfficeCode", purchaseRequest.Section);
//            return View(purchaseRequest);
//        }

//        // GET: PurchaseRequest/Edit/5
//        //public ActionResult Edit(int? id)
//        //{
//        //    if (id == null)
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    PurchaseRequest purchaseRequest = db.PurchaseRequest.Find(id);
//        //    if (purchaseRequest == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    ViewBag.Department = new SelectList(db.Offices, "ID", "OfficeCode", purchaseRequest.Department);
//        //    ViewBag.Section = new SelectList(db.Offices, "ID", "OfficeCode", purchaseRequest.Section);
//        //    return View(purchaseRequest);
//        //}

//        public ActionResult Edit()
//        {
//            ViewBag.Department = new SelectList(db.Offices, "ID", "OfficeName");
//            ViewBag.Section = new SelectList(db.Offices, "ID", "OfficeName");
//            return View();
//        }

//        // POST: PurchaseRequest/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,PurchaseRequestNo,Department,Section")] PurchaseRequest purchaseRequest)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(purchaseRequest).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.Department = new SelectList(db.Offices, "ID", "OfficeCode", purchaseRequest.Department);
//            ViewBag.Section = new SelectList(db.Offices, "ID", "OfficeCode", purchaseRequest.Section);
//            return View(purchaseRequest);
//        }

//        // GET: PurchaseRequest/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PurchaseRequest purchaseRequest = db.PurchaseRequest.Find(id);
//            if (purchaseRequest == null)
//            {
//                return HttpNotFound();
//            }
//            return View(purchaseRequest);
//        }

//        // POST: PurchaseRequest/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            PurchaseRequest purchaseRequest = db.PurchaseRequest.Find(id);
//            db.PurchaseRequest.Remove(purchaseRequest);
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
