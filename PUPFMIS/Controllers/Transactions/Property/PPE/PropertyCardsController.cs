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
//    public class PropertyCardsController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: PropertyCards
//        public ActionResult Index()
//        {
//            var propertyCards = db.PropertyCards.Include(p => p.FKOffice).Include(p => p.FKPPE);
//            return View(propertyCards.ToList());
//        }

//        // GET: PropertyCards/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PropertyCard propertyCard = db.PropertyCards.Find(id);
//            if (propertyCard == null)
//            {
//                return HttpNotFound();
//            }
//            return View(propertyCard);
//        }

//        // GET: PropertyCards/Create
//        public ActionResult Create()
//        {
//            ViewBag.IssueOffice = new SelectList(db.Offices, "ID", "OfficeCode");
//            ViewBag.Property = new SelectList(db.PPE, "ID", "ID");
//            return View();
//        }

//        // POST: PropertyCards/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,Date,Property,FundCluster,Reference,ReceiptQty,ReceiptUnitCost,IssueQty,IssueOffice,IssueOfficer,BalanceQty,Amount,Remarks")] PropertyCard propertyCard)
//        {
//            if (ModelState.IsValid)
//            {
//                db.PropertyCards.Add(propertyCard);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.IssueOffice = new SelectList(db.Offices, "ID", "OfficeCode", propertyCard.IssueOffice);
//            ViewBag.Property = new SelectList(db.PPE, "ID", "ID", propertyCard.Property);
//            return View(propertyCard);
//        }

//        // GET: PropertyCards/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PropertyCard propertyCard = db.PropertyCards.Find(id);
//            if (propertyCard == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.IssueOffice = new SelectList(db.Offices, "ID", "OfficeCode", propertyCard.IssueOffice);
//            ViewBag.Property = new SelectList(db.PPE, "ID", "ID", propertyCard.Property);
//            return View(propertyCard);
//        }

//        // POST: PropertyCards/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,Date,Property,FundCluster,Reference,ReceiptQty,ReceiptUnitCost,IssueQty,IssueOffice,IssueOfficer,BalanceQty,Amount,Remarks")] PropertyCard propertyCard)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(propertyCard).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.IssueOffice = new SelectList(db.Offices, "ID", "OfficeCode", propertyCard.IssueOffice);
//            ViewBag.Property = new SelectList(db.PPE, "ID", "ID", propertyCard.Property);
//            return View(propertyCard);
//        }

//        // GET: PropertyCards/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PropertyCard propertyCard = db.PropertyCards.Find(id);
//            if (propertyCard == null)
//            {
//                return HttpNotFound();
//            }
//            return View(propertyCard);
//        }

//        // POST: PropertyCards/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            PropertyCard propertyCard = db.PropertyCards.Find(id);
//            db.PropertyCards.Remove(propertyCard);
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
