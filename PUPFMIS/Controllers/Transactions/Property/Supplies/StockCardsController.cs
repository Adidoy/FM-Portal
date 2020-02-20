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
//    [Route("ops/property/stock-cards/{action}")]
//    public class StockCardsController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: StockCards
//        public ActionResult Index()
//        {
//            var stockCard = db.StockCard.Include(s => s.FKSupply);
//            return View(stockCard.ToList());
//        }

//        // GET: StockCards/Details/5
//        //public ActionResult Details(int? id)
//        //{
//        //    if (id == null)
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    StockCard stockCard = db.StockCard.Find(id);
//        //    if (stockCard == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(stockCard);
//        //}

//        public ActionResult Details()
//        {
//            return View();
//        }

//        // GET: StockCards/Create
//        public ActionResult Create()
//        {
//            ViewBag.SupplyID = new SelectList(db.SuppliesMaster, "ID", "StockNo");
//            return View();
//        }

//        // POST: StockCards/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,SupplyID,Date,Reference,ReceiptQty,ReceiptUnitCost,IssueQty,IssueUnitCost,BalanceQty,BalanceUnitCost")] StockCard stockCard)
//        {
//            if (ModelState.IsValid)
//            {
//                db.StockCard.Add(stockCard);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.SupplyID = new SelectList(db.SuppliesMaster, "ID", "StockNo", stockCard.SupplyID);
//            return View(stockCard);
//        }

//        // GET: StockCards/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            StockCard stockCard = db.StockCard.Find(id);
//            if (stockCard == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.SupplyID = new SelectList(db.SuppliesMaster, "ID", "StockNo", stockCard.SupplyID);
//            return View(stockCard);
//        }

//        // POST: StockCards/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,SupplyID,Date,Reference,ReceiptQty,ReceiptUnitCost,IssueQty,IssueUnitCost,BalanceQty,BalanceUnitCost")] StockCard stockCard)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(stockCard).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.SupplyID = new SelectList(db.SuppliesMaster, "ID", "StockNo", stockCard.SupplyID);
//            return View(stockCard);
//        }

//        // GET: StockCards/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            StockCard stockCard = db.StockCard.Find(id);
//            if (stockCard == null)
//            {
//                return HttpNotFound();
//            }
//            return View(stockCard);
//        }

//        // POST: StockCards/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            StockCard stockCard = db.StockCard.Find(id);
//            db.StockCard.Remove(stockCard);
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
