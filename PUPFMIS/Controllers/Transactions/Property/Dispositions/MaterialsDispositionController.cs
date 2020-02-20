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
//    [Route("ops/property/disposition/materials/{action}")]
//    public class MaterialsDispositionController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: MaterialsDisposition
//        public ActionResult Index()
//        {
//            var materialsDisposition = db.MaterialsDisposition.Include(m => m.FKLocation);
//            return View(materialsDisposition.ToList());
//        }

//        // GET: MaterialsDisposition/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            MaterialsDisposition materialsDisposition = db.MaterialsDisposition.Find(id);
//            if (materialsDisposition == null)
//            {
//                return HttpNotFound();
//            }
//            return View(materialsDisposition);
//        }

//        // GET: MaterialsDisposition/Create
//        public ActionResult Create()
//        {
//            ViewBag.PlaceOfStorage = new SelectList(db.Locations, "ID", "LocationName");
//            return View();
//        }

//        // POST: MaterialsDisposition/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,FundCluster,PlaceOfStorage,Date,CertifiedCorrect,DisposalApproved")] MaterialsDisposition materialsDisposition)
//        {
//            if (ModelState.IsValid)
//            {
//                db.MaterialsDisposition.Add(materialsDisposition);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.PlaceOfStorage = new SelectList(db.Locations, "ID", "LocationName", materialsDisposition.PlaceOfStorage);
//            return View(materialsDisposition);
//        }

//        // GET: MaterialsDisposition/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            MaterialsDisposition materialsDisposition = db.MaterialsDisposition.Find(id);
//            if (materialsDisposition == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.PlaceOfStorage = new SelectList(db.Locations, "ID", "LocationName", materialsDisposition.PlaceOfStorage);
//            return View(materialsDisposition);
//        }

//        // POST: MaterialsDisposition/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,FundCluster,PlaceOfStorage,Date,CertifiedCorrect,DisposalApproved")] MaterialsDisposition materialsDisposition)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(materialsDisposition).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.PlaceOfStorage = new SelectList(db.Locations, "ID", "LocationName", materialsDisposition.PlaceOfStorage);
//            return View(materialsDisposition);
//        }

//        // GET: MaterialsDisposition/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            MaterialsDisposition materialsDisposition = db.MaterialsDisposition.Find(id);
//            if (materialsDisposition == null)
//            {
//                return HttpNotFound();
//            }
//            return View(materialsDisposition);
//        }

//        // POST: MaterialsDisposition/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            MaterialsDisposition materialsDisposition = db.MaterialsDisposition.Find(id);
//            db.MaterialsDisposition.Remove(materialsDisposition);
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
