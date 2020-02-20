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
//    [Route("ops/procurement/apr/{action}")]
//    [Authorize]
//    public class AgencyProcurementRequestController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: AgencyProcurementRequest
//        public ActionResult Index()
//        {
//            return View(db.AgencyProcurementRequest.ToList());
//        }

//        // GET: AgencyProcurementRequest/Details/5
//        //public ActionResult Details(int? id)
//        //{
//        //    if (id == null)
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    AgencyProcurementRequest agencyProcurementRequest = db.AgencyProcurementRequest.Find(id);
//        //    if (agencyProcurementRequest == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(agencyProcurementRequest);
//        //}

//        public ActionResult Details()
//        {
//            return View();
//        }

//        // GET: AgencyProcurementRequest/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: AgencyProcurementRequest/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,AgencyControlNo,DatePrepared,APRControlNo")] AgencyProcurementRequest agencyProcurementRequest)
//        {
//            if (ModelState.IsValid)
//            {
//                db.AgencyProcurementRequest.Add(agencyProcurementRequest);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(agencyProcurementRequest);
//        }

//        // GET: AgencyProcurementRequest/Edit/5
//        //public ActionResult Edit(int? id)
//        //{
//        //    if (id == null)
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    AgencyProcurementRequest agencyProcurementRequest = db.AgencyProcurementRequest.Find(id);
//        //    if (agencyProcurementRequest == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(agencyProcurementRequest);
//        //}

//        public ActionResult Edit(int? id)
//        {
//            return View();
//        }

//        // POST: AgencyProcurementRequest/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,AgencyControlNo,DatePrepared,APRControlNo")] AgencyProcurementRequest agencyProcurementRequest)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(agencyProcurementRequest).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(agencyProcurementRequest);
//        }

//        // GET: AgencyProcurementRequest/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            AgencyProcurementRequest agencyProcurementRequest = db.AgencyProcurementRequest.Find(id);
//            if (agencyProcurementRequest == null)
//            {
//                return HttpNotFound();
//            }
//            return View(agencyProcurementRequest);
//        }

//        // POST: AgencyProcurementRequest/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            AgencyProcurementRequest agencyProcurementRequest = db.AgencyProcurementRequest.Find(id);
//            db.AgencyProcurementRequest.Remove(agencyProcurementRequest);
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
