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
//    [Route("ops/property/ppe/{action}")]
//    public class PPEsController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        [Route("ops/property/ppe/{type}/index")]
//        public ActionResult Index(string type)
//        {
//            ViewBag.Type = type;
//            var PPE = db.PPE.Include(p => p.FKItem).Include(p => p.FKUACS);
//            return View(PPE.ToList());
//        }

//        [Route("ops/property/ppe/assigned-property/index")]
//        public ActionResult AssignedPropertyIndex()
//        {
//            var PARDetails = db.PARDetails;
//            return View(PARDetails.ToList());
//        }

//        [Route("ops/propert/ppe/assigned-property/{property_number}/transfer-location")]
//        public ActionResult TransferLocation(string property_number)
//        {
//            ViewBag.CurrentLocation = new SelectList(db.Locations, "ID", "LocationName");
//            ViewBag.TransferLocation = new SelectList(db.Locations, "ID", "LocationName");
//            return View();
//        }

//        [Route("ops/property/ppe/assigned-property/{id}/view-par-details")]
//        public ActionResult DetailsPAR(int id)
//        {
//            PropertyAcknowledgementVM PropertyAcknowldegementVM = new PropertyAcknowledgementVM();
//            PropertyAcknowldegementVM.PARHeader = db.PARHeader.Find(id);
//            PropertyAcknowldegementVM.PARDetails = db.PARDetails.Where(d => d.PARNo == id);
//            return View(PropertyAcknowldegementVM);
//        }

//        [Route("ops/property/ppe/{id}/property-cards/")]
//        public ActionResult PropertyCards()
//        {
//            return View();
//        }

//        // GET: PPEs/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PPE pPE = db.PPE.Find(id);
//            if (pPE == null)
//            {
//                return HttpNotFound();
//            }
//            return View(pPE);
//        }

//        // GET: PPEs/Create
//        public ActionResult Create()
//        {
//            ViewBag.Unit = new SelectList(db.UOM, "ID", "UnitName");
//            ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName");
//            ViewBag.UACSObjectAccountCode = new SelectList(db.ChartOfAccounts, "ID", "UACS");
//            return View();
//        }

//        // POST: PPEs/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,ItemID,UACSObjectAccountCode,UsefulLife,DepreciationRate")] PPE pPE)
//        {
//            if (ModelState.IsValid)
//            {
//                db.PPE.Add(pPE);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName", pPE.ItemID);
//            ViewBag.UACSObjectAccountCode = new SelectList(db.ChartOfAccounts, "ID", "UACS", pPE.UACSObjectAccountCode);
//            return View(pPE);
//        }

//        [Route("ops/property/ppe/{id}/assign-property-number")]
//        public ActionResult PropertyNumberAssignment(int? id)
//        {
//            //if (id == null)
//            //{
//            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            //}
//            PPE PPE = db.PPE.Find(id);
//            //if (PPE == null)
//            //{
//            //    return HttpNotFound();
//            //}
//            return View();
//        }

//        // GET: PPEs/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PPE pPE = db.PPE.Find(id);
//            if (pPE == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName", pPE.ItemID);
//            ViewBag.UACSObjectAccountCode = new SelectList(db.ChartOfAccounts, "ID", "UACS", pPE.UACSObjectAccountCode);
//            return View(pPE);
//        }

//        // POST: PPEs/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,ItemID,UACSObjectAccountCode,UsefulLife,DepreciationRate")] PPE pPE)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(pPE).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName", pPE.ItemID);
//            ViewBag.UACSObjectAccountCode = new SelectList(db.ChartOfAccounts, "ID", "UACS", pPE.UACSObjectAccountCode);
//            return View(pPE);
//        }

//        // GET: PPEs/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PPE pPE = db.PPE.Find(id);
//            if (pPE == null)
//            {
//                return HttpNotFound();
//            }
//            return View(pPE);
//        }

//        // POST: PPEs/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            PPE pPE = db.PPE.Find(id);
//            db.PPE.Remove(pPE);
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
