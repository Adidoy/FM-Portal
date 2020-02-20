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
//    [Route("ops/procurement/ppmp-approval/{action}")]
//    [Authorize]
//    public class PPMPApprovalsController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: PPMPApproval
//        public ActionResult Index()
//        {
//            return View(db.PPMPApproval.ToList());
//        }

//        public ActionResult Review()
//        {
//            return View();
//        }

//        // GET: PPMPApproval/Details/5
//        public ActionResult Details()
//        {
//            //PPMPApproval PPMPApproval = db.PPMPApproval.Find();
//            return View();
//        }

//        // GET: PPMPApproval/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: PPMPApproval/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,Action,Remarks,ActionTakenBy,CreatedAt")] PPMPApproval pPMPApproval)
//        {
//            if (ModelState.IsValid)
//            {
//                db.PPMPApproval.Add(pPMPApproval);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(pPMPApproval);
//        }

//        // GET: PPMPApproval/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PPMPApproval pPMPApproval = db.PPMPApproval.Find(id);
//            if (pPMPApproval == null)
//            {
//                return HttpNotFound();
//            }
//            return View(pPMPApproval);
//        }

//        // POST: PPMPApproval/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,Action,Remarks,ActionTakenBy,CreatedAt")] PPMPApproval pPMPApproval)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(pPMPApproval).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(pPMPApproval);
//        }

//        // GET: PPMPApproval/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PPMPApproval pPMPApproval = db.PPMPApproval.Find(id);
//            if (pPMPApproval == null)
//            {
//                return HttpNotFound();
//            }
//            return View(pPMPApproval);
//        }

//        // POST: PPMPApproval/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            PPMPApproval pPMPApproval = db.PPMPApproval.Find(id);
//            db.PPMPApproval.Remove(pPMPApproval);
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
