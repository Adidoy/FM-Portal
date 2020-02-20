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
//    [Route("ops/property/physical-count/{type}/{action}")]
//    public class PhysicalCountController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: PhysicalCount
//        public ActionResult Index(string type)
//        {
//            ViewBag.Type = type;
//            return View(db.PhysicalCount.ToList());
//        }

//        // GET: PhysicalCount/Details/5
//        public ActionResult Details(int? id, string type)
//        {
//            ViewBag.Type = type;
//            //if (id == null)
//            //{
//            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            //}
//            //PhysicalCount physicalCount = db.PhysicalCount.Find(id);
//            //if (physicalCount == null)
//            //{
//            //    return HttpNotFound();
//            //}
//            return View();
//        }

//        // GET: PhysicalCount/Create
//        public ActionResult Create(string type)
//        {
//            ViewBag.Type = type;
//            if(type == "properties")
//            {
//                ViewBag.EndUser = new SelectList(db.Offices, "ID", "OfficeName");
//                ViewBag.Location = new SelectList(db.Locations, "ID", "LocationName");
//            }
//            return View();
//        }

//        // POST: PhysicalCount/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,PhysicalInventoryNo,FundCluster,CountFor,DateOfCount,ProcessedBy,VerifiedBy")] PhysicalCount physicalCount)
//        {
//            if (ModelState.IsValid)
//            {
//                db.PhysicalCount.Add(physicalCount);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(physicalCount);
//        }

//        // GET: PhysicalCount/Edit/5
//        public ActionResult Edit(int? id, string type)
//        {
//            ViewBag.Type = type;
//            //if (id == null)
//            //{
//            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            //}
//            //PhysicalCount physicalCount = db.PhysicalCount.Find(id);
//            //if (physicalCount == null)
//            //{
//            //    return HttpNotFound();
//            //}
//            return View();
//        }

//        // POST: PhysicalCount/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,PhysicalInventoryNo,FundCluster,CountFor,DateOfCount,ProcessedBy,VerifiedBy")] PhysicalCount physicalCount)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(physicalCount).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(physicalCount);
//        }

//        // GET: PhysicalCount/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PhysicalCount physicalCount = db.PhysicalCount.Find(id);
//            if (physicalCount == null)
//            {
//                return HttpNotFound();
//            }
//            return View(physicalCount);
//        }

//        // POST: PhysicalCount/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            PhysicalCount physicalCount = db.PhysicalCount.Find(id);
//            db.PhysicalCount.Remove(physicalCount);
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
