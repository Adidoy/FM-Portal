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
//    [Route("ops/property/inspections/{action}")]
//    public class InspectionsController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: Inspections
//        public ActionResult Index()
//        {
//            var inspectionHeader = db.InspectionHeader.Include(i => i.FKPOReference).Include(supplier => supplier.FKPOReference.FKSupplier);
//            return View(inspectionHeader.ToList());
//        }

//        // GET: Inspections/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            InspectionHeader inspectionHeader = db.InspectionHeader.Find(id);
//            if (inspectionHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(inspectionHeader);
//        }

//        // GET: Inspections/Create
//        public ActionResult Create()
//        {
//            ViewBag.POReference = new SelectList(db.POHeader, "ID", "PONumber");
//            return View();
//        }

//        // POST: Inspections/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,IARNo,POReference,InspectionPersonnel,InspectionDate,OverallRemarks")] InspectionHeader inspectionHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.InspectionHeader.Add(inspectionHeader);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.POReference = new SelectList(db.POHeader, "ID", "PONumber", inspectionHeader.POReference);
//            return View(inspectionHeader);
//        }

//        // GET: Inspections/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            InspectionHeader inspectionHeader = db.InspectionHeader.Find(id);
//            if (inspectionHeader == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.POReference = new SelectList(db.POHeader, "ID", "PONumber", inspectionHeader.POReference);
//            return View(inspectionHeader);
//        }

//        // POST: Inspections/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,IARNo,POReference,InspectionPersonnel,InspectionDate,OverallRemarks")] InspectionHeader inspectionHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(inspectionHeader).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.POReference = new SelectList(db.POHeader, "ID", "PONumber", inspectionHeader.POReference);
//            return View(inspectionHeader);
//        }

//        // GET: Inspections/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            InspectionHeader inspectionHeader = db.InspectionHeader.Find(id);
//            if (inspectionHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(inspectionHeader);
//        }

//        // POST: Inspections/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            InspectionHeader inspectionHeader = db.InspectionHeader.Find(id);
//            db.InspectionHeader.Remove(inspectionHeader);
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
