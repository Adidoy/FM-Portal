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
//    [Route("ops/procurement/pipeline/{action}")]
//    public class ProcurementPipelineController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: ProcurementPipeline
//        public ActionResult Index()
//        {
//            return View(db.ProcurementPipelineHeader.ToList());
//        }

//        // GET: ProcurementPipeline/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ProcurementPipelineHeader procurementPipelineHeader = db.ProcurementPipelineHeader.Find(id);
//            if (procurementPipelineHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(procurementPipelineHeader);
//        }

//        // GET: ProcurementPipeline/Create
//        public ActionResult Create()
//        {
//            ViewBag.ProcurementCategory = new SelectList(db.ItemSubCategory, "ID", "ProcurementCategoryName");
//            return View();
//        }

//        // POST: ProcurementPipeline/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,ProcurementLineNumber,ProcurementMode,ProjectCoordinator,ProcurmentStart")] ProcurementPipelineHeader procurementPipelineHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.ProcurementPipelineHeader.Add(procurementPipelineHeader);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(procurementPipelineHeader);
//        }

//        // GET: ProcurementPipeline/Edit/5
//        public ActionResult Update(int? id)
//        {
//            //if (id == null)
//            //{
//            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            //}
//            ProcurementPipelineHeader procurementPipelineHeader = db.ProcurementPipelineHeader.Find(id);
//            //if (procurementPipelineHeader == null)
//            //{
//            //    return HttpNotFound();
//            //}
//            ViewBag.ProcurementCategory = new SelectList(db.ItemSubCategory, "ID", "ProcurementCategoryName");
//            return View(procurementPipelineHeader);
//        }

//        // POST: ProcurementPipeline/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,ProcurementLineNumber,ProcurementMode,ProjectCoordinator,ProcurmentStart")] ProcurementPipelineHeader procurementPipelineHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(procurementPipelineHeader).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(procurementPipelineHeader);
//        }

//        // GET: ProcurementPipeline/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ProcurementPipelineHeader procurementPipelineHeader = db.ProcurementPipelineHeader.Find(id);
//            if (procurementPipelineHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(procurementPipelineHeader);
//        }

//        // POST: ProcurementPipeline/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            ProcurementPipelineHeader procurementPipelineHeader = db.ProcurementPipelineHeader.Find(id);
//            db.ProcurementPipelineHeader.Remove(procurementPipelineHeader);
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
