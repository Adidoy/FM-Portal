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
//    [Route("ops/procurement/app/{action}")]
//    [Authorize]
//    public class APPController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: APP
//        public ActionResult Index()
//        {
//            return View(db.APPHeader.ToList());
//        }

//        [Route("ops/procurement/app/{id}/details")]
//        public ActionResult Details(int? id)
//        {
//            //if (id == null)
//            //{
//            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            //}
//            APPHeader APPHeader = db.APPHeader.Find(id);
//            //if (aPPHeader == null)
//            //{
//            //    return HttpNotFound();
//            //}
//            return View(APPHeader);
//        }


//        [Route("ops/budget/annual-procurement-plan/indicative-app")]
//        public ActionResult CreateIndicative()
//        {
//            ViewData["MOOE"] = db.ExpenditureObjects.Where(d => d.OperatingExpenditure != 0).ToList();
//            return View();
//        }

//        public ActionResult CreateOtherProjects()
//        {
//            ViewData["MOOE"] = db.ExpenditureObjects.Where(d => d.OperatingExpenditure != 0).ToList();
//            return View();
//        }

//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: APP/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,ReferenceNo,FiscalYear,CreatedAt,PercentImplemented,ApprovedAt")] APPHeader aPPHeader)
//        {
//            //if (ModelState.IsValid)
//            //{
//            //    db.APPHeader.Add(aPPHeader);
//            //    db.SaveChanges();
//            //    return RedirectToAction("Index");
//            //}

//            //return View(aPPHeader);
//            return Redirect("/ops/procurement/app/details");
//        }

//        // GET: APP/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            APPHeader aPPHeader = db.APPHeader.Find(id);
//            if (aPPHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aPPHeader);
//        }

//        // POST: APP/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,ReferenceNo,FiscalYear,CreatedAt,PercentImplemented,ApprovedAt")] APPHeader aPPHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(aPPHeader).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(aPPHeader);
//        }

//        // GET: APP/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            APPHeader aPPHeader = db.APPHeader.Find(id);
//            if (aPPHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(aPPHeader);
//        }

//        // POST: APP/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            APPHeader aPPHeader = db.APPHeader.Find(id);
//            db.APPHeader.Remove(aPPHeader);
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
