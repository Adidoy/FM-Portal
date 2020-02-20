//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;

//namespace PUPFMIS.Controllers.Transactions.Property.PPE
//{
//    public class PPEReturnsController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: PPEReturns
//        public ActionResult Index()
//        {
//            return View(db.PPEReturn.ToList());
//        }

//        // GET: PPEReturns/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PPEReturn pPEReturn = db.PPEReturn.Find(id);
//            if (pPEReturn == null)
//            {
//                return HttpNotFound();
//            }
//            return View(pPEReturn);
//        }

//        // GET: PPEReturns/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: PPEReturns/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,EndUser,DateReturned")] PPEReturn pPEReturn)
//        {
//            if (ModelState.IsValid)
//            {
//                db.PPEReturn.Add(pPEReturn);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(pPEReturn);
//        }

//        // GET: PPEReturns/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PPEReturn pPEReturn = db.PPEReturn.Find(id);
//            if (pPEReturn == null)
//            {
//                return HttpNotFound();
//            }
//            return View(pPEReturn);
//        }

//        // POST: PPEReturns/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,EndUser,DateReturned")] PPEReturn pPEReturn)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(pPEReturn).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(pPEReturn);
//        }

//        // GET: PPEReturns/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PPEReturn pPEReturn = db.PPEReturn.Find(id);
//            if (pPEReturn == null)
//            {
//                return HttpNotFound();
//            }
//            return View(pPEReturn);
//        }

//        // POST: PPEReturns/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            PPEReturn pPEReturn = db.PPEReturn.Find(id);
//            db.PPEReturn.Remove(pPEReturn);
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
